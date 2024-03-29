﻿using AutoMapper;
using SuggestionsApp.Models.DataModels;
using SuggestionsApp.WebUI.ViewModels;

namespace SuggestionsApp.WebUI.Mappings
{
    public class StateMapping : Profile
    {
        public StateMapping()
        {
            CreateMap<State, StateViewModel>();
            CreateMap<StateViewModel, State>();
        }
    }

    public class StatesListMapping : Profile
    {
        public StatesListMapping()
        {
            CreateMap<State, StatesListViewModel>();
            CreateMap<StatesListViewModel, State>();
        }
    }
}
