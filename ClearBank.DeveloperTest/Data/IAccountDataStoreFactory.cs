﻿namespace ClearBank.DeveloperTest.Data
{
    public interface IAccountDataStoreFactory
    {
        IAccountDataStore BuildAccountDataStore();
    }
}