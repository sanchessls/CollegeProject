﻿using System;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.SignalRServerSide
{
#region IClock
    public interface IFeature
    {
        Task FeatureUpdated(int featureId, string id);
        Task StatusFeatureUpdated(int sessionId, string id);
    }
#endregion
}
