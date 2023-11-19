using System;
using UnityEngine;

namespace MultiplayerProtocol
{
    /// <summary>
    /// Special interpretation of the promise pattern which splits up the Then() part into two distinct actions:
    /// First, <see cref="ThenSuccess" /> receives the result, then allow the promise to run some more code before calling
    /// <see cref="Then"/>. If the promise runs <see cref="Catch" />, both <see cref="ThenSuccess"/> and
    /// <see cref="Then"/> will not be called. After all other handlers, <see cref="Finally"/> is always called.
    /// </summary>
    public class RequestPromise
    {
        private bool hasResult;
        private Exception error;
        private bool finished;

        private Action successHandler;
        private Action<Exception> errorHandler;
        private Action afterHandler;
        private Action finallyHandler;

        public RequestPromise(Action<Action, Action<Exception>, Action> resolver)
        {
            try
            {
                resolver(AcceptSuccess, AcceptError, Finish);
            }
            catch (Exception e)
            {
                if (!hasResult && !finished)
                {
                    AcceptError(e);
                }
                else
                {
                    Debug.LogError(e);
                }
            }
        }

        private void AcceptSuccess()
        {
            if (hasResult) throw new InvalidOperationException("Promise already has a result");
            if (finished) throw new InvalidOperationException("Promise is already finished");
            hasResult = true;
            if (successHandler != null) successHandler();
        }

        private void AcceptError(Exception e)
        {
            if (hasResult) throw new InvalidOperationException("Promise already has a result");
            if (finished) throw new InvalidOperationException("Promise is already finished");
            error = e;
            hasResult = true;
            finished = true;
            if (errorHandler != null) errorHandler(e);
            if (finallyHandler != null) finallyHandler();
        }

        private void Finish()
        {
            if (finished) throw new InvalidOperationException("Promise is already finished");
            finished = true;
            if (afterHandler != null) afterHandler();
            if (finallyHandler != null) finallyHandler();
        }

        public RequestPromise ThenSuccess(Action handler)
        {
            if (hasResult)
            {
                handler();
                return this;
            }

            successHandler = handler;
            return this;
        }

        public RequestPromise Then(Action handler)
        {
            if (finished && error == null)
            {
                handler();
                return this;
            }

            afterHandler = handler;
            return this;
        }

        public RequestPromise Catch(Action<Exception> handler)
        {
            if (finished && error != null)
            {
                handler(error);
                return this;
            }

            errorHandler = handler;
            return this;
        }

        public RequestPromise Finally(Action handler)
        {
            if (finished)
            {
                handler();
                return this;
            }

            finallyHandler = handler;
            return this;
        }
    }

    /// <summary>
    /// Special interpretation of the promise pattern which splits up the Then() part into two distinct actions:
    /// First, <see cref="ThenAccept" /> receives the result, then allow the promise to run some more code before calling
    /// <see cref="Then"/>. If the promise runs <see cref="Catch" />, both <see cref="ThenAccept"/> and
    /// <see cref="Then"/> will not be called. After all other handlers, <see cref="Finally"/> is always called.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class RequestPromise<TResult>
    {
        private bool hasResult;
        private TResult result;
        private Exception error;
        private bool finished;

        private Action<TResult> resultHandler;
        private Action<Exception> errorHandler;
        private Action afterHandler;
        private Action finallyHandler;

        public RequestPromise(Action<Action<TResult>, Action<Exception>, Action> resolver)
        {
            try
            {
                resolver(AcceptResult, AcceptError, Finish);
            }
            catch (Exception e)
            {
                if (!hasResult && !finished)
                {
                    AcceptError(e);
                }
                else
                {
                    Debug.LogError(e);
                }
            }
        }

        private void AcceptResult(TResult result)
        {
            if (hasResult) throw new InvalidOperationException("Promise already has a result");
            if (finished) throw new InvalidOperationException("Promise is already finished");
            this.result = result;
            hasResult = true;
            if (resultHandler != null) resultHandler(result);
        }

        private void AcceptError(Exception e)
        {
            if (hasResult) throw new InvalidOperationException("Promise already has a result");
            if (finished) throw new InvalidOperationException("Promise is already finished");
            error = e;
            hasResult = true;
            finished = true;
            if (errorHandler != null) errorHandler(e);
            if (finallyHandler != null) finallyHandler();
        }

        private void Finish()
        {
            if (finished) throw new InvalidOperationException("Promise is already finished");
            finished = true;
            if (afterHandler != null) afterHandler();
            if (finallyHandler != null) finallyHandler();
        }

        public RequestPromise<TResult> ThenAccept(Action<TResult> handler)
        {
            if (hasResult)
            {
                handler(result);
                return this;
            }

            resultHandler = handler;
            return this;
        }

        public RequestPromise<TResult> Then(Action handler)
        {
            if (finished && error == null)
            {
                handler();
                return this;
            }

            afterHandler = handler;
            return this;
        }

        public RequestPromise<TResult> Catch(Action<Exception> handler)
        {
            if (finished && error != null)
            {
                handler(error);
                return this;
            }

            errorHandler = handler;
            return this;
        }

        public RequestPromise<TResult> Finally(Action handler)
        {
            if (finished)
            {
                handler();
                return this;
            }

            finallyHandler = handler;
            return this;
        }
    }
}