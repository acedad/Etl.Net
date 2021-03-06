﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Paillave.Etl.Reactive.Core
{
    public class DeferredWrapperPushObservable<T> : IDeferredPushObservable<T>
    {
        private Action _start;
        private IPushObservable<T> _observable;
        private Synchronizer _synchronizer = null;
        public DeferredWrapperPushObservable(IPushObservable<T> observable, Action start, Synchronizer synchronizer = null)
        {
            _observable = observable;
            _start = start;
            _synchronizer = synchronizer;
        }
        public void Start()
        {
            if (_synchronizer == null)
                _start();
            else
                using (_synchronizer.WaitBeforeProcess())
                    _start();
        }

        public IDisposable Subscribe(Action<T> onPush)
        {
            return _observable.Subscribe(onPush);
        }

        public IDisposable Subscribe(Action<T> onPush, Action onComplete)
        {
            return _observable.Subscribe(onPush, onComplete);
        }

        public IDisposable Subscribe(Action<T> onPush, Action onComplete, Action<Exception> onException)
        {
            return _observable.Subscribe(onPush, onComplete, onException);
        }

        public IDisposable Subscribe(ISubscription<T> subscription)
        {
            return _observable.Subscribe(subscription);
        }
    }
}
