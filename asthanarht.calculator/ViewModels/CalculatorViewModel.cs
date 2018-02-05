using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace asthanarht.calculator
{
    public class CalculatorViewModel : BaseViewModel
    {
        private decimal? _operandOne;
        private decimal? _operandTwo;
        private Operator? _operation;
        private string _displayValue;
        private string _displayText;
        private string _previousVal;
        private bool _hasOperation;
        private string _newDisplay = string.Empty;
        private bool _isDigitEnable = true;
       
        public CalculatorViewModel()
        {

            _hasOperation = false;

            DigitComamnd = new Command<string>(DigitCommandExecute);
            OperatorCommand = new Command<string>(OperatorCommandExecute);
            DeleteCommand = new Command(DeleteCommandExecute);
            ClearEntryCommand = new Command(ClearEntryCommandExecute);
            ClearCommand = new Command(ClearCommandExecute);
            ComputeCommand = new Command(ComputeCommandExecute);

            DisplayValue = string.Empty;
            DisplayText = string.Empty;

        }

        public string DisplayValue
        {
            get { return _displayValue; }
            set
            {
                _displayValue = value;
                OnPropertyChanged(nameof(DisplayValue));
            }
        }
        public bool IsDigitEnable 
        {
            get { return _isDigitEnable; }
            set
            {
                _isDigitEnable = value;
                OnPropertyChanged(nameof(IsDigitEnable));
            }
        }
        public string DisplayText
        {
            get { return _displayText; }
            set
            {
                _displayText = value;
                OnPropertyChanged(nameof(DisplayText));
            }
        }

        public Operator? Operation
        {
            get { return _operation; }
            set
            {
                _operation = value;
                OnPropertyChanged(nameof(Operation));
            }
        }

        public decimal? OperandOne
        {
            get { return _operandOne; }
            set
            {
                _operandOne = value;
                OnPropertyChanged(nameof(OperandOne));
  
            }
        }

        public decimal? OperandTwo
        {
            get { return _operandTwo; }
            set
            {
                _operandTwo = value;
                OnPropertyChanged(nameof(OperandTwo));
            }
        }

        public ICommand DigitComamnd { get; private set; }
        public ICommand OperatorCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand ClearEntryCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand ComputeCommand { get; private set; }

        private void DigitCommandExecute(string value)
        {
            decimal result;

           
            if (_hasOperation)
            {
               // Clear();
                _hasOperation = false;
            }
                _newDisplay = _newDisplay + value;

           
          
           // string newDisplay = ;

            if (Decimal.TryParse(_newDisplay, out result))
            {
                if (_operation.HasValue)
                {
                    _operandTwo = result;
                }
                else
                {
                    _operandOne = result;
                }
                //ShowDisplay(newDisplay);
                //DisplayValue = newDisplay;
                DisplayText = DisplayText + value;
            }
        }

        private void ShowDisplay(string val)
        {
            DisplayText = DisplayText + val;
        }
        private void OperatorCommandExecute(string op)
        {
            Operator? operation;
            IsDigitEnable = true;
            if (_hasOperation)
            {
                DisplayText = _previousVal;
                _hasOperation = false;
            }
            //ShowDisplay(op);
            switch (op)
            {
                case "+":
                    operation = Operator.Addition;
                    break;
                case "-":
                    operation = Operator.Subtraction;
                    break;
                case "*":
                    operation = Operator.Multiplication;
                    break;
                case "/":
                    operation = Operator.Division;
                    break;
                default:
                    throw new ArgumentException("Invalid Operator!");
            }

            if (_operandTwo.HasValue)
            {
                
                var calculation = Calculate();
                if (calculation != 0)
                {
                    _operandOne = calculation;
                    _operandTwo = null;
                    _operation = operation;
                    _previousVal = calculation.ToString();
                    _newDisplay = string.Empty;
                    DisplayText= calculation.ToString()+ op ;
                }
                else
                {
                    Clear();
                }
            }
            else if (_operandOne.HasValue)
            {
                _operation = operation;
                _operandTwo = null;
                DisplayText = DisplayText + op;
                _newDisplay = string.Empty;
                DisplayValue = string.Empty;
            }
        }

        private  decimal ConvertStringToDecimal( string val)
        {
            decimal result;
            Decimal.TryParse(val, out result);
            return result;
        }

        private void DeleteCommandExecute()
        {
            if (_hasOperation)
                return;

            if (!String.IsNullOrWhiteSpace(DisplayText))
            {
                DisplayText = DisplayText.Remove(DisplayText.Length - 1);
            }
        }

        private void ClearEntryCommandExecute()
        {
            if (_operandTwo.HasValue)
            {
                _operandTwo = null;
            }
            else
            {
                _operation = null;
                _operandOne = null;
            }

            DisplayValue = string.Empty;
            DisplayText = string.Empty;
        }

        private void ClearCommandExecute()
        {
            Clear();
            IsDigitEnable = true;
        }

        private void Clear()
        {
            _operandOne = null;
            _operandTwo = null;
            _operation = null;

            DisplayValue = string.Empty;
            DisplayText = string.Empty;
            _newDisplay = string.Empty;
            _previousVal = string.Empty;
        }

        private void ComputeCommandExecute()
        {
             _previousVal = Calculate().ToString();
            DisplayValue = _previousVal;
            //DisplayText = string.Empty;
           //DisplayText = value;
            _operandTwo = null;
            _operandOne = ConvertStringToDecimal(_previousVal);
            _hasOperation = true;
            IsDigitEnable = false;
        }

        public decimal GetPercentValue(decimal? percentage, decimal baseValue)
        {
            if (percentage == null)
                return 0;

            return baseValue * (percentage.Value / 100);
        }

        public decimal Calculate()
        {
            if (_operandOne.HasValue
                && _operation.HasValue
                && _operandTwo.HasValue)
            {
                switch (_operation.Value)
                {
                    case Operator.Addition:
                        return RoundUpbyNumber((_operandOne.Value + _operandTwo.Value),10);
                    case Operator.Subtraction:
                        return RoundUpbyNumber((_operandOne.Value - _operandTwo.Value),10);
                    case Operator.Multiplication:
                        return RoundUpbyNumber((_operandOne.Value * _operandTwo.Value),5);
                    case Operator.Division:
                        return RoundUpbyNumber((_operandOne.Value / _operandTwo.Value),5);
                    default:
                        return 0;
                }
            }
            return 0;
        }


    public decimal RoundUpbyNumber(decimal number,int roundNumber)
        {
            return (decimal)Math.Round(number,roundNumber);
        }
    }


    public enum Operator
    {
        Addition,
        Subtraction,
        Division,
        Multiplication
    }
}
