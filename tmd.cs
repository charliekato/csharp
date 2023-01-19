public  class timeData
{
    Queue<int> dataFifo = new Queue<int>();
    private static int timeDataEncode(int timeint, int laneNo, int goalFlag)
    {
        return laneNo * 1000000 + (goalFlag * 10000000) + timeint;
    }
    private static void timeDataDecode(int timedata, ref int timeint, ref int laneNo, ref int goalFlag)
    {
        goalFlag = timedata / 10000000;
        laneNo = (timedata % 10000000) / 1000000;
        timeint = timedata % 1000000;
    }
    public void push()
    {
        dataFifo.Enqueue(0);
    }
    public void push(int timeint, int laneNo, int goalFlag)
    {
        dataFifo.Enqueue(timeDataEncode(timeint, laneNo, goalFlag));
    }
    public bool pop(ref int timeint, ref int laneNo, ref int goalFlag)
    {
        if (dataFifo.Count>0)
        {
            timeDataDecode(dataFifo.Dequeue(), ref timeint, ref laneNo, ref goalFlag);
            return true;
        }
        return false;
        
    }
}
