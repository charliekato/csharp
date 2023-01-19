    public static void readandFifoPush()
    {
        byte[] buffer = new byte[100];
        byte[] charbyte = new byte[20];
        const byte stx = 2;
        const byte etx = 3;
        int howmanyread;
        int counter = -1;
        int laneNo=0;

        string mytime = "";
        try
        {
            while (true)
            {
                howmanyread = _serialPort.Read(buffer, 0, 54); // 54=18*3
                //Console.WriteLine("{0} bytes read", howmanyread);
                for (int j = 0; j < howmanyread; j++)
                {
                    if (buffer[j] == stx)
                    {
                        counter = 0;
                    }
                    else if (buffer[j] == etx)
                    {
                        counter = -1;  
                        if (is_start(charbyte))
                        {
                            tmd.push();
                        }
                        
                        if (is_lap(charbyte))
                        {
                            laneNo = charbyte[2] - 48;
                            
                            mytime = Encoding.ASCII.GetString(charbyte, 5, 8);
                            tmd.push(timestr2int(mytime), laneNo, 0);
                           
                        }
                        if (is_goal(charbyte))
                        {
                            laneNo = charbyte[2] - 48;
                           
                            mytime = Encoding.ASCII.GetString(charbyte, 5, 8);
                            tmd.push(timestr2int(mytime), laneNo, 1);
                        }
                       
                    } else if (counter>=0)
                    {
                        charbyte[counter++] = buffer[j];
                        //Console.Write("{0} ,{1}", buffer[j], Encoding.ASCII.GetString(buffer, j,1));
                        if (counter > 16)
                        {
                            Console.WriteLine("error counter reaches 17.");
                            counter = -1;
                        }
                    }

                }
            }

           
        }
        catch (TimeoutException e) {
            Console.WriteLine(e.Message);   
        }
    }
