﻿using System;

namespace int32.Utils.ServiceHandler.Contracts
{
    public interface IServiceDelete<T>
    {
        void Delete(int id);
        void Delete(T item);
        void Delete(Predicate<T> predicate);
    }

    public interface IServiceDeleteParam<T> where T : IServiceParameter
    {
        void Delete(T param);
    }
}
