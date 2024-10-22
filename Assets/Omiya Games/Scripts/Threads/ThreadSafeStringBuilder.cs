﻿using System.Text;

namespace OmiyaGames
{
    public class ThreadSafeStringBuilder : ThreadSafe<StringBuilder>
    {
        public ThreadSafeStringBuilder() : base(new StringBuilder()) { }

        public int Length
        {
            get
            {
                lock (ThreadLock)
                {
                    return value.Length;
                }
            }
        }

        public override string ToString()
        {
            lock (ThreadLock)
            {
                return value.ToString();
            }
        }

        public void Clear()
        {
            lock (ThreadLock)
            {
                value.Clear();
            }
        }

        public void Append(string append)
        {
            lock (ThreadLock)
            {
                value.Append(append);
            }
        }

        public void Insert(int index, string insert)
        {
            lock(ThreadLock)
            {
                value.Insert(index, insert);
            }
        }

        public void AppendLine()
        {
            lock (ThreadLock)
            {
                value.AppendLine();
            }
        }
    }
}
