using Castle.DynamicProxy;

namespace Blax;
public class StateInterceptor : IInterceptor
{
    public void Intercept(IInvocation invocation)
    {
        // Check if it's a setter method
        if (invocation.Method.IsSpecialName && invocation.Method.Name.StartsWith("set_"))
        {
            // Check if the property has the [Observable] attribute
            var propertyName = invocation.Method.Name[4..];
            var property = invocation.TargetType.GetProperty(propertyName);
            if (property == null) return;

            if (Attribute.IsDefined(property, typeof(ObservableAttribute)))
            {
                // Get the old value using the getter
                object? oldValue = property.GetValue(invocation.InvocationTarget);

                // Get the new value from the method arguments
                object? newValue = invocation.Arguments.Length > 0 ? invocation.Arguments[0] : null;

                // Proceed with the original invocation to actually set the new value
                invocation.Proceed();

                // Check if the old value is the same as the new value
                if (Equals(oldValue, newValue))
                {
                    return; // Do not notify subscribers if the value didn't change
                }

                // Trigger subscribers
                var observableState = invocation.InvocationTarget as ObservableState;
                observableState?.NotifySubscribers();
            }
        }
        else
        {
            // Proceed with the original invocation for non-setter methods
            invocation.Proceed();
        }
    }

}
