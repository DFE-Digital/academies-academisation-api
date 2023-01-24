Dynamics lookup conversion ->

private const string EqualityStatement1 = "That the Secretary of State's decision is unlikely to disproportionately affect any particular person or group who share protected characteristics";
    private const string EqualityStatement2 = "That there are some impacts but on balance the changes will not disproportionately affect any particular person or group who share protected characteristics";

    public static bool? ConvertSurplusOrDeficit(this int? value) => value switch
    {
        907660000 => false,
        907660001 => true,
        _ => null
    };
    
    public static string ConvertApplicationType(this int? value) => value switch
    {
        100000001 => "JoinMat",
        907660000 => "FormMat",
        907660001 => "FormSat",
        _ => null
    };
    
    public static string ConvertApplicationRole(this int? role) => role switch
    {
        907660000 => "Headteacher",
        907660001 => "ChairGovernor",
        907660002 => "Other",
        _ => null
    };
    
    public static bool? ConvertDynamicsIntBool(this int? value) => value switch
    {
        907660000 => true,
        907660001 => false,
        _ => null
    };

    public static string ConvertFundsPaidTo(this int? paid) => paid switch
    {
        907660000 => "School",
        907660001 => "Trust",
        _ => null
    };

    public static bool? ConvertDynamicsEqualitiesImpactAssessmentIntBool(this int? value) => value switch
    {
        907660000 => true,
        907660001 => true,
        907660002 => false,
        _ => null
    };

    public static string ConvertEqualitiesImpactAssessmentDetails(this int? selectedStatement) => selectedStatement switch
    {
        907660000 => EqualityStatement1,
        907660001 => EqualityStatement2,
        907660002 => null,
        _ => null
    };