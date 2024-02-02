using System.Runtime.InteropServices;

namespace TrainSwitching.Logic;

public class TrainStation
{
    public Track[] Tracks { get; }

     public TrainStation()
    {
        // int[] tracks = Tracks;
        // TODO: Implement this method
        throw new NotImplementedException();
    }
    /// <summary>
    /// Tries to apply the given operation to the train station.
    /// </summary>
    /// <param name="op">Operation to apply</param>
    /// <returns>Returns true if the operation could be applied, otherwise false</returns>
    public bool TryApplyOperation(SwitchingOperation op)
    {
        // TODO: Implement this method
        if (op.TrackNumber > 10 || op.TrackNumber < 1)
        {
            return false;
        }

        if ((op.TrackNumber is 9 or 10) && op.Direction == Constants.DIRECTION_EAST)
        {
            return false;
        }

        if (op.OperationType == Constants.OPERATION_TRAIN_LEAVE && Tracks[op.TrackNumber].Wagons.Count == 0)
        {
            return false;
        }

        if (op.OperationType == Constants.OPERATION_REMOVE && (Tracks[op.TrackNumber].Wagons.Count < op.NumberOfWagons))
        {
            return false;
        }

        if (op.OperationType == Constants.OPERATION_TRAIN_LEAVE)
        {
            bool locomotiveIsThere = false;
            for (int i = 0; i < Tracks[op.TrackNumber].Wagons.Count; i++)
            {
                if (Tracks[op.TrackNumber].Wagons[i] == Constants.WAGON_TYPE_LOCOMOTIVE)
                {
                    locomotiveIsThere = true;
                }
            }
            if (!locomotiveIsThere) { return false; }
        }

        switch (op.OperationType)
        {
            case Constants.OPERATION_ADD:
                if (op.Direction == Constants.DIRECTION_EAST)
                {
                    Tracks[op.TrackNumber].Wagons.Add((int)op.WagonType!);
                }
                else
                {
                    Tracks[op.TrackNumber].Wagons.Insert(0, (int)op.WagonType!);
                }
                break;
            case Constants.OPERATION_TRAIN_LEAVE:
                Tracks[op.TrackNumber].Wagons.Clear();
                break;
            case Constants.OPERATION_REMOVE:
                for (int i = 0; i < op.NumberOfWagons; i++){
                    Tracks[op.TrackNumber].Wagons.RemoveAt(Tracks[op.TrackNumber].Wagons.Count -1);
                }
                break;
        }
        return true;
    }

    /// <summary>
    /// Calculates the checksum of the train station.
    /// </summary>
    /// <returns>The calculated checksum</returns>
    /// <remarks>
    /// See readme.md for details on how to calculate the checksum.
    /// </remarks>
    public int CalculateChecksum()
    {
        // TODO: Implement this method
        throw new NotImplementedException();
    }
}