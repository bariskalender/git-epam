using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using ValidationAttributes;

namespace ValidationServiceTask;

public sealed class ValidationService<T>
{
    private readonly Dictionary<string, List<ValidationResult>> validationInfo = new ();

    public IReadOnlyDictionary<string, List<ValidationResult>> ValidationInfo => this.validationInfo;

    public bool IsValid(T? instance)
    {
        this.validationInfo.Clear();

        if (instance is null)
        {
            return true;
        }

        var type = instance.GetType();
        var nullabilityContext = new NullabilityInfoContext();

        bool isValid = true;

        foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public))
        {
            var attrs = field.GetCustomAttributes<ValidationAttribute>(inherit: true).ToArray();
            if (attrs.Length == 0)
            {
                continue;
            }

            var value = field.GetValue(instance);
            var n = nullabilityContext.Create(field);
            bool memberNullable = n.ReadState == NullabilityState.Nullable;

            isValid &= this.ValidateMember(field.Name, value, memberNullable, attrs);
        }

        foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!prop.CanRead)
            {
                continue;
            }

            var attrs = prop.GetCustomAttributes<ValidationAttribute>(inherit: true).ToArray();
            if (attrs.Length == 0)
            {
                continue;
            }

            var value = prop.GetValue(instance);
            var n = nullabilityContext.Create(prop);
            bool memberNullable = n.ReadState == NullabilityState.Nullable;

            isValid &= this.ValidateMember(prop.Name, value, memberNullable, attrs);
        }

        return isValid;
    }

    private bool ValidateMember(string memberName, object? value, bool memberNullable, ValidationAttribute[] attributes)
    {
        if (value is null)
        {
            bool ok = true;

            foreach (var attr in attributes)
            {
                if (attr is RequiredAttribute && !attr.IsValid(null))
                {
                    ok = false;
                    this.AddError(memberName, attr.FormatErrorMessage(memberName));
                }
            }

            if (!memberNullable)
            {
                ok = false;
                this.AddError(memberName, $"{memberName} is required.");
            }

            return ok;
        }

        bool allOk = true;

        foreach (var attr in attributes)
        {
            if (attr.IsValid(value))
            {
                continue;
            }

            allOk = false;
            this.AddError(memberName, attr.FormatErrorMessage(memberName));
        }

        return allOk;
    }

    private void AddError(string memberName, string message)
    {
        if (!this.validationInfo.TryGetValue(memberName, out var list))
        {
            list = new List<ValidationResult>();
            this.validationInfo[memberName] = list;
        }

        list.Add(new ValidationResult(memberName, message));
    }
}
