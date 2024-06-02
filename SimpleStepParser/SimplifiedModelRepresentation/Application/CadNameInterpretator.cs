using SimpleStepParser.SimplifiedModelRepresentation.Domain;

namespace SimpleStepParser.SimplifiedModelRepresentation.Application;

internal static class CadNameInterpretator
{
    public static CadName GetCadNameByHeader(string? header)
    {
        if(header == null)
        {
            return CadName.Unknown;
        }

        if(header.Contains("SOLIDWORKS", StringComparison.InvariantCultureIgnoreCase))
        {
            return CadName.SolidWorks;
        }
        else if(header.Contains("CREO PARAMETRIC", StringComparison.InvariantCultureIgnoreCase))
        {
            return CadName.CreoParametric;
        }
        else if(header.Contains("ST-Developer", StringComparison.InvariantCultureIgnoreCase))
        {
            return CadName.SiemensNx;
        }
        else if(header.Contains("C3D Converter", StringComparison.InvariantCultureIgnoreCase))
        {
            return CadName.Kompas;
        }
        else
        {
            return CadName.Unknown;
        }
    }

    public static StepVersion GetStepVersionByHeader(string? header)
    {
        if (header == null)
        {
            return StepVersion.Unknown;
        }

        else if (header.Contains("AP203", StringComparison.InvariantCultureIgnoreCase))
        {
            return StepVersion.AP203;
        }
        else if (header.Contains("AP214", StringComparison.InvariantCultureIgnoreCase))
        {
            return StepVersion.AP214;
        }
        else if (header.Contains("AP242", StringComparison.InvariantCultureIgnoreCase))
        {
            return StepVersion.AP242;
        }
        else
        {
            return StepVersion.Unknown;
        }
    }
}
