using Application.DTOs.States.Messages;
using Application.DTOs.States.Responses;

namespace Application.Mapping
{
    internal static class StateMapping
    {
        internal static StatesMessage MapResponseToMessage(this List<StatesResponse> list)
        {
            var stateMessage = new StatesMessage();

            foreach (StatesResponse state in list)
            {
                stateMessage.StatesList.Add(new State
                {
                    Id = state.Id,
                    Sigla = state.Sigla,
                    Nome = state.Nome,
                    Regiao = new Regiao
                    {
                        Id = state.Regiao.Id,
                        Sigla = state.Regiao.Sigla,
                        Nome = state.Regiao.Nome
                    }
                });
            }

            return stateMessage;
        }
    }
}
