
using System;
using System.Timers;


namespace Pomelo.DotNetClient
{
    public class HeartBeatService
    {
        private int interval;
        public int timeout;
        private Timer timer;
        private DateTime lastTime;
        private Protocol protocol;

        public HeartBeatService(int interval, Protocol protocol)
        {
            this.interval = interval * 1000;
            this.protocol = protocol;
        }

        internal void resetTimeout()
        {
            this.timeout = 0;
            this.lastTime = DateTime.Now;
        }

        public void sendHeartBeat(object source, ElapsedEventArgs e)
        {
            this.timeout = (int)(DateTime.Now - this.lastTime).TotalMilliseconds;
            if (this.timeout > this.interval * 2)
                this.protocol.getPomeloClient().disconnect();
            else
                this.protocol.send(PackageType.PKG_HEARTBEAT);
        }

        public void start()
        {
            if (this.interval < 1000)
                return;
            this.timer = new Timer();
            this.timer.Interval = (double)this.interval;
            this.timer.Elapsed += new ElapsedEventHandler(this.sendHeartBeat);
            this.timer.Enabled = true;
            this.timeout = 0;
            this.lastTime = DateTime.Now;
        }

        public void stop()
        {
            if (this.timer == null)
                return;
            this.timer.Enabled = false;
            this.timer.Dispose();
        }
    }
}
