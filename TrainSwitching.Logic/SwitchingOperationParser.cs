using System.ComponentModel;
using System.Globalization;

namespace TrainSwitching.Logic;

public static class SwitchingOperationParser
{
    /// <summary>
    /// Parses a line of input into a <see cref="SwitchingOperation"/>.
    /// </summary>
    /// <param name="inputLine">Line to parse. See readme.md for details</param>
    /// <returns>The parsed switching operation</returns>
    public static SwitchingOperation Parse(string inputLine)
    {
        SwitchingOperation myOperation = new();
        int countTo = inputLine.IndexOf(',');
        for (int i = 0; i <= countTo; i++)
        {
            if (int.TryParse(inputLine[i].ToString(), out int number))
            {
                if (int.TryParse(inputLine[i + 1].ToString(), out int number2))
                {
                    myOperation.TrackNumber = int.Parse(number.ToString() + number2.ToString());
                }
                else
                {
                    myOperation.TrackNumber = number;
                    
                }
            }
            if (i == countTo)
            {
                char operation = inputLine[i + 2];

                myOperation.OperationType = operation switch
                {
                    'a' => Constants.OPERATION_ADD,
                    'r' => Constants.OPERATION_REMOVE,
                    't' => Constants.OPERATION_TRAIN_LEAVE,
                    _ => throw new InvalidOperationException("This operation is not valid."),
                };
            }
        }
        if (inputLine.Contains("East"))
        {
            myOperation.Direction = Constants.DIRECTION_EAST;
        }
        else
        {
            myOperation.Direction = Constants.DIRECTION_WEST;
        }

        switch (myOperation.OperationType)
        {
            case Constants.OPERATION_ADD:
                char firstChar = inputLine[inputLine.IndexOf("add") + 4];
                myOperation.WagonType = firstChar switch
                {
                    'P' => Constants.WAGON_TYPE_PASSENGER,
                    'L' => Constants.WAGON_TYPE_LOCOMOTIVE,
                    'F' => Constants.WAGON_TYPE_FREIGHT,
                    'C' => Constants.WAGON_TYPE_CAR_TRANSPORT,
                    _ => throw new InvalidOperationException("This wagon type is not valid."),
                };
                break;
            case Constants.OPERATION_REMOVE:
                myOperation.NumberOfWagons = int.Parse(inputLine[inputLine.IndexOf("remove") + 7].ToString());
                break;
            default:
                break;

        }
        return myOperation;

    }
}