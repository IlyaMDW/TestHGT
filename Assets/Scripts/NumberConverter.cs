using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class NumberConverter : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputText;
    [SerializeField] private TextMeshProUGUI _outputText;

    private Dictionary<ColorType, Color> _colors;
    private Dictionary<int, string> _divisors;
    private Dictionary<string, string> _replacers;

    private void Awake()
    {
        FillColors();
        FillDivisors();
        FillReplacers();
    }

    public void ConvertButton()
    {
        if (string.IsNullOrEmpty(_inputText.text))
        {
            ShowText("PLEASE ENTER ROW OF NUMBERS", ColorType.Warning);
            return;
        }

        //string testText = "1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 60, 105, 420";
        string[] numbersText = _inputText.text.Split(',');
        for (int i = 0; i < numbersText.Length; i++)
        {
            numbersText[i] = numbersText[i].Trim();
        }

        if (TryConvertRowToIntArray(numbersText, out int[] numbers))
        {
            StringBuilder convertedRow = new StringBuilder();
            StringBuilder convertedNumber = new StringBuilder();
            bool isFirstChanging = true;

            for (int i = 0; i < numbers.Length; i++)
            {
                convertedNumber.Clear();
                foreach (var divisor in _divisors)
                {
                    if (CheckRemainderByZero(numbers[i], divisor.Key))
                    {
                        if (isFirstChanging)
                        {
                            isFirstChanging = false;
                            convertedNumber.Append(divisor.Value);
                        }
                        else
                        {
                            convertedNumber.Append("-");
                            convertedNumber.Append(divisor.Value);
                        }
                    }
                }
                isFirstChanging = true;
                if (convertedNumber.Length == 0)
                    convertedNumber.Append(numbers[i].ToString());
                else
                    ReplaceValues(convertedNumber);
                if (i != numbers.Length - 1)
                    convertedNumber.Append(", ");

                convertedRow.Append(convertedNumber);
            }

            ShowText(convertedRow.ToString(), ColorType.Default);
        }
        else
            ShowText("AN UNSUITABLE FORMAT STRING", ColorType.Error);
    }

    private bool TryConvertRowToIntArray(string[] numbersText, out int[] numbers)
    {
        try
        {
            List<int> numberList = new List<int>();
            int currentNumber;
            foreach (string number in numbersText)
            {
                currentNumber = int.Parse(number);
                numberList.Add(currentNumber);
            }
            numbers = numberList.ToArray();
            return true;
        }
        catch
        {
            numbers = new int[0];
            return false;
        }
    }

    private bool CheckRemainderByZero(int dividend, int divisor)
    {
        int remainder = dividend % divisor;
        if (remainder == 0)
            return true;
        else
            return false;
    }

    private void ReplaceValues(StringBuilder number)
    {
        number.Replace("fizz-buzz", _replacers["fizz-buzz"]);
        foreach (var item in _replacers)
        {
            if (number.Length == item.Key.Length && number.Equals(item.Key))
                number.Replace(item.Key, item.Value);
        }
    }

    private void ShowText(string text, ColorType colorType)
    {
        _outputText.text = text;
        _outputText.color = _colors[colorType];
    }

    private void FillColors()
    {
        _colors = new Dictionary<ColorType, Color>();
        _colors[ColorType.Error] = Color.red;
        _colors[ColorType.Warning] = Color.yellow;
        _colors[ColorType.Default] = Color.black;
    }
    private void FillDivisors()
    {
        _divisors = new Dictionary<int, string>();
        _divisors[3] = "fizz";
        _divisors[5] = "buzz";
        _divisors[4] = "muzz";
        _divisors[7] = "guzz";
    }
    private void FillReplacers()
    {
        _replacers = new Dictionary<string, string>();
        _replacers["fizz"] = "dog";
        _replacers["buzz"] = "cat";
        _replacers["fizz-buzz"] = "good-boy";
    }
}
