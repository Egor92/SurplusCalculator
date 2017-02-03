using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using SurplusCalculator.Infrastructure;
using SurplusCalculator.Models;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace SurplusCalculator.ViewModels
{
    public class MainViewModel : ReactiveObject, IDisposable
    {
        #region Fields

        private const string TimeFormat = @"yyyy-MM-dd hh-mm-ss";
        private readonly IFileSelector _fileSelector;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly Models.SurplusCalculator _calculator = new Models.SurplusCalculator();

        #endregion

        #region Ctor

        public MainViewModel(IFileSelector fileSelector)
        {
            _fileSelector = fileSelector;
        }

        #endregion

        #region IsBusy

        private bool _isBusy;

        public bool IsBusy
        {
            get { return _isBusy; }
            private set { this.RaiseAndSetIfChanged(ref _isBusy, value); }
        }

        #endregion

        #region SourceFilePath

        private string _sourceFilePath;

        public string SourceFilePath
        {
            get { return _sourceFilePath; }
            private set { this.RaiseAndSetIfChanged(ref _sourceFilePath, value); }
        }

        #endregion

        #region SourceItemLength

        private int _sourceItemLength = 6000;

        public int SourceItemLength
        {
            get { return _sourceItemLength; }
            set { this.RaiseAndSetIfChanged(ref _sourceItemLength, value); }
        }

        #endregion

        #region ResultsString

        private string _resultsString;

        public string ResultsString
        {
            get { return _resultsString; }
            set { this.RaiseAndSetIfChanged(ref _resultsString, value); }
        }

        #endregion

        #region SelectSourceFileCommand

        private ICommand _selectSourceFileCommand;

        public ICommand SelectSourceFileCommand
        {
            get { return _selectSourceFileCommand ?? (_selectSourceFileCommand = GetSelectSourceFileCommand()); }
        }

        private ReactiveCommand GetSelectSourceFileCommand()
        {
            var command = ReactiveCommand.Create(SelectSourceFile, null, DispatcherScheduler.Current);
            _disposables.Add(command);
            return command;
        }

        private void SelectSourceFile()
        {
            var selectedFilePath = _fileSelector.SelectFile();
            if (selectedFilePath == null)
                return;
            SourceFilePath = selectedFilePath;
        }

        #endregion

        #region StartCalculationCommand

        private ICommand _startCalculationCommand;

        public ICommand StartCalculationCommand
        {
            get { return _startCalculationCommand ?? (_startCalculationCommand = GetStartCalculationCommand()); }
        }

        private ReactiveCommand GetStartCalculationCommand()
        {
            var canExecuteObservable = CanStartCalculationExecuteObservable();
            var command = ReactiveCommand.Create(StartCalculation, canExecuteObservable, DispatcherScheduler.Current);
            _disposables.Add(command);
            return command;
        }

        private IObservable<bool> CanStartCalculationExecuteObservable()
        {
            return this.WhenAnyValue(x => x.SourceFilePath, x => x.SourceItemLength)
                       .Throttle(TimeSpan.FromMilliseconds(200), DispatcherScheduler.Current)
                       .Select(_ => CanStartCalculationExecute());
        }

        private bool CanStartCalculationExecute()
        {
            return !string.IsNullOrWhiteSpace(SourceFilePath) && SourceItemLength > 0;
        }

        private void StartCalculation()
        {
            IsBusy = true;

            Dictionary<int, int> targetItemCountsByLengths = null;
            try
            {
                targetItemCountsByLengths = GetTargetItemCountsByLengths();
            }
            catch (Exception e)
            {
                ResultsString = $"Ошибка при считывании файла:\r\n\r\n{e.Message}";
                IsBusy = false;
                return;
            }

            Task.Run(() =>
            {
                IList<ItemInfo> itemInfos = new List<ItemInfo>();
                try
                {
                    itemInfos = _calculator.Calculate(SourceItemLength, targetItemCountsByLengths);
                }
                catch (Exception e)
                {
                    ResultsString = $"Ошибка при расчёте:\r\n\r\n{e.Message}";
                    IsBusy = false;
                    return;
                }

                int count = itemInfos.Count;
                var resultAsString = itemInfos.Select((x, i) => $"{ItemInfoHelper.GetFreeLength(x)}. Излишек: {ItemInfoHelper.GetHash(x)}")
                                              .Aggregate(string.Empty, (x1, x2) => $"{x2}{Environment.NewLine}{x1}");
                var directoryName = Path.GetDirectoryName(SourceFilePath);
                var fileName = Path.GetFileNameWithoutExtension(SourceFilePath);
                var targetFilePath = Path.Combine(directoryName, $"{fileName}. result {DateTime.Now.ToString(TimeFormat)}.txt");
                File.WriteAllText(targetFilePath, resultAsString);

                ResultsString = $"Вычисления произведены успешно.\r\n\r\nТребуется {count} заготовок.\r\n\r\n{resultAsString}";

                IsBusy = false;
            });
        }

        private Dictionary<int, int> GetTargetItemCountsByLengths()
        {
            return File.ReadAllLines(SourceFilePath)
                       .Select(x => x.Trim())
                       .Where(x => !string.IsNullOrWhiteSpace(x))
                       .Select(x => x.Split(';'))
                       .ToDictionary(x => Convert.ToInt32(x[0]), x => Convert.ToInt32(x[1]));
        }

        #endregion

        #region Implementation of IDisposable

        private bool _isDisposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            if (disposing)
            {
                _disposables.Dispose();
            }

            _isDisposed = true;
        }

        ~MainViewModel()
        {
            Dispose(false);
        }

        #endregion
    }
}
