﻿using System;
using System.Threading.Tasks;

namespace ScrumPokerPlanning.SignalRServerSide
{
#region IClock
    public interface IClock
    {
        Task ShowTime(DateTime currentTime);
    }
#endregion
}
