namespace TimeManager.Database.Interfaces
{
    internal interface ITimeManagerQuery<T>
    {
        public T Execute();
    }
}
