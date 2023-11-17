using TurnBase.DBLayer.Interfaces;
using TurnBase.DBLayer.Models;
using TurnBase.DBLayer.Repositories;
using TurnBase.Server.Enums;
using TurnBase.Server.Models;

namespace TurnBase.Server.Core.Services
{
    internal class ParameterService
    {
        private static ParameterDTO[] _parameters = new ParameterDTO[0];
        private static ParameterDTO[] _clientSendParameters = new ParameterDTO[0];

        public static ParameterDTO[] Parameters => _parameters;
        public static ParameterDTO[] ClientSendParameters => _clientSendParameters;

        public static void Initialize()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                _parameters = uow.GetRepository<TblParameter>().Select(y => new ParameterDTO
                {
                    Id = (Parameters)y.Id,
                    BoolValue = y.BoolValue,
                    FloatValue = y.FloatValue,
                    IntValue = y.IntValue,
                    SendToClient = y.SendToClient,
                }).ToArray();

                _clientSendParameters = _parameters.Where(y => y.SendToClient).ToArray();
            }
        }

        public static ParameterDTO GetParameter(Parameters parameter)
        {
            return _parameters.FirstOrDefault(y => y.Id == parameter);
        }

        public static int GetIntValue(Parameters parameter)
        {
            return GetParameter(parameter).IntValue.GetValueOrDefault(0);
        }
        public static double GetDoubleValue(Parameters parameter)
        {
            return GetParameter(parameter).FloatValue.GetValueOrDefault(0);
        }
        public static bool GetBoolValue(Parameters parameter)
        {
            return GetParameter(parameter).BoolValue.GetValueOrDefault(false);
        }
    }
}
