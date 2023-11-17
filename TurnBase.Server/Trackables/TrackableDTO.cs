﻿using Newtonsoft.Json;
using TurnBase.Server.Server.Interfaces;
using TurnBase.Server.Server.ServerModels;

namespace TurnBase.Server.Trackables
{
    public abstract class TrackableDTO : IChangeItem
    {
        private IChangeHandler _changeHandler;
        public void SetChangeHandler(IChangeHandler changeHandler)
        {
            this._changeHandler = changeHandler;
        }

        public void SetAsChanged()
        {
            _changeHandler.AddChanges(this);
        }

        public abstract SocketResponse GetResponse();
    }
}
