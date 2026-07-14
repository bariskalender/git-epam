using System;

namespace ValidationServiceTask;

public sealed class ValidationResult
{
    public ValidationResult(string memberName, string validationMessage)
    {
        this.MemberName = memberName ?? throw new ArgumentNullException(nameof(memberName));
        this.ValidationMessage = validationMessage ?? throw new ArgumentNullException(nameof(validationMessage));
    }

    public string MemberName { get; }

    public string ValidationMessage { get; }
}
