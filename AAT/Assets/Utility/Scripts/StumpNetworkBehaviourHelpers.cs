using System;
using Fusion;

namespace Utility.Scripts
{
    public static class StumpNetworkBehaviourHelpers
    {
        public static bool StumpTryGetBehaviour(this Behaviour behaviour, Type type, out Behaviour returnBehaviour)
        {
            returnBehaviour = default;
            object[] parameters = { null };
            var tryGetMethod = typeof(Behaviour).GetMethod(nameof(Behaviour.TryGetBehaviour));
            var tryGetTypeMethod = tryGetMethod.MakeGenericMethod(type);
            var result = tryGetTypeMethod.Invoke(behaviour, parameters);
            
            var returnValue = (bool) result;
            if (returnValue)
            {
                returnBehaviour = (Behaviour) parameters[0];
            }

            return returnValue;
        }

        public static Behaviour StumpAddBehaviour(this Behaviour behaviour, Type type)
        {
            var addMethod = typeof(Behaviour).GetMethod(nameof(Behaviour.AddBehaviour));
            var addTypeMethod = addMethod.MakeGenericMethod(type);
            return (Behaviour) addTypeMethod.Invoke(behaviour, null);
        }
    }
}
