using System;
using UnityEngine;
using VContainer;

namespace TFrameWork.VContainer
{
    public static class VContainerExtensions
    {
        public static RegistrationBuilder RegisterEntryPoint(this IContainerBuilder builder, Type type, Lifetime lifetime)
        {
            var registrationBuilder = builder.Register(type, lifetime);
            if (type.IsSubclassOf(typeof(MonoBehaviour)))
            {
                registrationBuilder = registrationBuilder.As(typeof(MonoBehaviour));
            }
            return registrationBuilder.AsImplementedInterfaces();
        }
    }
}
