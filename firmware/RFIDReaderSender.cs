using firmware.Command;
using System.Collections.Concurrent;

namespace firmware
{
    internal class RFIDReaderSender
    {
        private readonly RFIDReader m_Reader;

        private Thread m_Thread;

        private readonly BlockingCollection<BaseCommand> m_Commands = new BlockingCollection<BaseCommand>();

        public RFIDReaderSender(RFIDReader reader)
        {
            m_Reader = reader;
            m_Thread = new Thread(ProcSend);
        }

        public void Open()
        {
            if (m_Thread.ThreadState == ThreadState.Running)
            {
                return;
            }

            m_Thread = new Thread(ProcSend);
            m_Thread.Start();
        }

        public bool Send(BaseCommand command)
        {
            try
            {
                m_Commands.Add(command);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void ProcSend()
        {
            foreach (BaseCommand command in m_Commands.GetConsumingEnumerable())
            {
                m_Reader.SendBytes(command.GetBytes());
                Thread.Sleep(10);
            }
        }

        public void Close()
        {
            m_Commands.CompleteAdding();
            m_Thread.Join();
        }
    }
}
