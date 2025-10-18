using Application.DTOs.States.Messages;
using Application.DTOs.States.Responses;

namespace Application.Mapping
{
    internal static class StateMapping
    {
        internal static List<StatesMessage> MapResponseToMessage(this List<StatesResponse> list)
        {
            var stateMessageList = new List<StatesMessage>();
            foreach (StatesResponse state in list)
            {
                stateMessageList.Add(new StatesMessage
                {
                    Id = state.Id,
                    Sigla = state.Sigla,
                    Nome = state.Nome,
                    Regiao = new RegiaoMessage
                    {
                        Id = state.Regiao.Id,
                        Sigla = state.Regiao.Sigla,
                        Nome = state.Regiao.Nome
                    }
                });
            }

            return stateMessageList;
        }
    }
}
