using System.Runtime.InteropServices;
using System.Security.Authentication;

namespace TrainSwitching.Logic;

public class TrainStation
{
    public Track[] Tracks { get; }

    public TrainStation()
    {
        Tracks = new Track[10];
        for (int i = 0; i < 10; i++)
        {
            Tracks[i] = new Track();
            Tracks[i].TrackNumber = i + 1;
        }

    }
    /// <summary>
    /// Tries to apply the given operation to the train station.
    /// </summary>
    /// <param name="op">Operation to apply</param>
    /// <returns>Returns true if the operation could be applied, otherwise false</returns>
    public bool TryApplyOperation(SwitchingOperation op)
    {
        // TODO: Implement this method
        var trackID = op.TrackNumber - 1;
        if (op.TrackNumber > 10 || op.TrackNumber < 1)
        {
            return false;
        }

        if ((op.TrackNumber is 9 or 10) && op.Direction == Constants.DIRECTION_EAST)
        {
            return false;
        }

        if (op.OperationType == Constants.OPERATION_TRAIN_LEAVE && Tracks[trackID].Wagons.Count == 0)
        {
            return false;
        }

        if (op.OperationType == Constants.OPERATION_REMOVE && (Tracks[trackID].Wagons.Count < op.NumberOfWagons))
        {
            return false;
        }

        if (op.OperationType == Constants.OPERATION_TRAIN_LEAVE)
        {
            bool locomotiveIsThere = false;
            for (int i = 0; i < Tracks[trackID].Wagons.Count; i++)
            {
                if (Tracks[trackID].Wagons[i] == Constants.WAGON_TYPE_LOCOMOTIVE)
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
                    Tracks[trackID].Wagons.Add((int)op.WagonType!);
                }
                else
                {
                    Tracks[trackID].Wagons.Insert(0, (int)op.WagonType!);
                }
                break;
            case Constants.OPERATION_TRAIN_LEAVE:
                Tracks[trackID].Wagons.Clear();
                break;
            case Constants.OPERATION_REMOVE:
                for (int i = 0; i < op.NumberOfWagons; i++)
                {
                    if (op.Direction == Constants.DIRECTION_EAST)
                    {
                        Tracks[trackID].Wagons.RemoveAt(Tracks[trackID].Wagons.Count - 1);
                    }
                    else
                    {
                        Tracks[trackID].Wagons.RemoveAt(0);
                    }
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
        const int PASSENGER = 1;
        const int LOCOMOTIVE = 10;
        const int FREIGHT = 20;
        const int CAR = 30;
        var sums = new int[Tracks.Length];
        int tracksum;

        for (int i = 0; i < Tracks.Length; i++)
        {
            tracksum = 0;
            for (int j = 0; j < Tracks[i].Wagons.Count; j++)
            {
                tracksum += Tracks[i].Wagons[j] switch
                {
                    Constants.WAGON_TYPE_PASSENGER => PASSENGER,
                    Constants.WAGON_TYPE_LOCOMOTIVE => LOCOMOTIVE,
                    Constants.WAGON_TYPE_FREIGHT => FREIGHT,
                    Constants.WAGON_TYPE_CAR_TRANSPORT => CAR,
                    _ => throw new InvalidOperationException("This wagon type is invalid.")
                };
            }
            sums[i] = tracksum * (i + 1);
        }

        return sums.Sum();

    }
}