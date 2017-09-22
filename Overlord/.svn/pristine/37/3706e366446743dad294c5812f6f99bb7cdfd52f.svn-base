

using System;
using System.Net.Sockets;

namespace Pomelo.DotNetClient
{
    public class Transporter
    {
        private StateObject stateObject = new StateObject();
        private byte[] headBuffer = new byte[4];
        public const int HeadLength = 4;
        private Socket socket;
        private Action<byte[]> messageProcesser;
        private TransportState transportState;
        private byte[] buffer;
        private int bufferOffset;
        private int pkgLength;
        internal Action onDisconnect;

        public Transporter(Socket socket, Action<byte[]> processer)
        {
            this.socket = socket;
            this.messageProcesser = processer;
            this.transportState = TransportState.readHead;
        }

        public void start()
        {
            this.receive();
        }

        public void send(byte[] buffer)
        {
            if (this.transportState == TransportState.closed)
                return;
            this.socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(this.sendCallback), (object)this.socket);
        }

        private void sendCallback(IAsyncResult asyncSend)
        {
            if (this.transportState == TransportState.closed)
                return;
            this.socket.EndSend(asyncSend);
        }

        public void receive()
        {
            this.socket.BeginReceive(this.stateObject.buffer, 0, this.stateObject.buffer.Length, SocketFlags.None, new AsyncCallback(this.endReceive), (object)this.stateObject);
        }

        internal void close()
        {
            this.transportState = TransportState.closed;
        }

        private void endReceive(IAsyncResult asyncReceive)
        {
            if (this.transportState == TransportState.closed)
                return;
            StateObject stateObject = (StateObject)asyncReceive.AsyncState;
            Socket socket = this.socket;
            try
            {
                int limit = socket.EndReceive(asyncReceive);
                if (limit > 0)
                {
                    this.processBytes(stateObject.buffer, 0, limit);
                    if (this.transportState == TransportState.closed)
                        return;
                    this.receive();
                }
                else
                {
                    if (this.onDisconnect == null)
                        return;
                    this.onDisconnect();
                }
            }
            catch (SocketException ex)
            {
                if (this.onDisconnect == null)
                    return;
                this.onDisconnect();
            }
        }

        internal void processBytes(byte[] bytes, int offset, int limit)
        {
            if (this.transportState == TransportState.readHead)
            {
                this.readHead(bytes, offset, limit);
            }
            else
            {
                if (this.transportState != TransportState.readBody)
                    return;
                this.readBody(bytes, offset, limit);
            }
        }

        private bool readHead(byte[] bytes, int offset, int limit)
        {
            int length1 = limit - offset;
            int length2 = 4 - this.bufferOffset;
            if (length1 >= length2)
            {
                this.writeBytes(bytes, offset, length2, this.bufferOffset, this.headBuffer);
                this.pkgLength = ((int)this.headBuffer[1] << 16) + ((int)this.headBuffer[2] << 8) + (int)this.headBuffer[3];
                this.buffer = new byte[4 + this.pkgLength];
                this.writeBytes(this.headBuffer, 0, 4, this.buffer);
                offset += length2;
                this.bufferOffset = 4;
                this.transportState = TransportState.readBody;
                if (offset <= limit)
                    this.processBytes(bytes, offset, limit);
                return true;
            }
            this.writeBytes(bytes, offset, length1, this.bufferOffset, this.headBuffer);
            this.bufferOffset += length1;
            return false;
        }

        private void readBody(byte[] bytes, int offset, int limit)
        {
            int length = this.pkgLength + 4 - this.bufferOffset;
            if (offset + length <= limit)
            {
                this.writeBytes(bytes, offset, length, this.bufferOffset, this.buffer);
                offset += length;
                this.messageProcesser(this.buffer);
                this.bufferOffset = 0;
                this.pkgLength = 0;
                if (this.transportState != TransportState.closed)
                    this.transportState = TransportState.readHead;
                if (offset >= limit)
                    return;
                this.processBytes(bytes, offset, limit);
            }
            else
            {
                this.writeBytes(bytes, offset, limit - offset, this.bufferOffset, this.buffer);
                this.bufferOffset += limit - offset;
                this.transportState = TransportState.readBody;
            }
        }

        private void writeBytes(byte[] source, int start, int length, byte[] target)
        {
            this.writeBytes(source, start, length, 0, target);
        }

        private void writeBytes(byte[] source, int start, int length, int offset, byte[] target)
        {
            for (int index = 0; index < length; ++index)
                target[offset + index] = source[start + index];
        }

        private void print(byte[] bytes, int offset, int length)
        {
            for (int index = offset; index < length; ++index)
                Console.Write(Convert.ToString(bytes[index], 16) + " ");
            Console.WriteLine();
        }
    }
}
