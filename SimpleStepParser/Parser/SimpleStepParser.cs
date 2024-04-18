using SimpleStepParser.StepFileRepresentation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleStepParser.Parser
{
    public static class SimpleStepParser
    {
        public static void ReadStepFile(string path)
        {
            if(!File.Exists(path))
            {
                throw new FileNotFoundException();
            }
            var file = File.ReadAllLines(path, Encoding.UTF8);
            var representation = GetStepRepresentation(file);
            ;
        }

        private static StepRepresentation GetStepRepresentation(string[] stepFile)
        {
            if (!stepFile[0].ToUpper().Contains("ISO-10303-21;") ||
                !stepFile[stepFile.Length - 1]
                .ToUpper().Contains("END-ISO-10303-21;"))
            {
                throw new ArgumentException("This is not a ISO-10303-21 file");
            }

            int i = 0;
            //Skip to header
            for (; i < stepFile.Length; i++)
            {
                if (stepFile[i].ToUpper().Contains("HEADER;"))
                {
                    i++;
                    break;
                }
            }
            //Saving header
            StringBuilder header = new();
            for (; i < stepFile.Length; i++)
            {
                if (stepFile[i].ToUpper().Contains("ENDSEC;"))
                {
                    i++;
                    break;
                }
                header.AppendLine(stepFile[i]);
            }
            //Skip to data
            for (; i < stepFile.Length; i++)
            {
                if (stepFile[i].ToUpper().Contains("DATA;"))
                {
                    i++;
                    break;
                }
            }
            //Saving data
            List<StepEntity> data = new();
            Regex entityStart = new Regex(@"^#(?<id>\d*)=(?<body>.*)");
            for (; i < stepFile.Length; i++)
            {
                //if the line is the end of data section
                if (stepFile[i].ToUpper().Contains("ENDSEC;"))
                {
                    break;
                }

                var m = entityStart.Match(stepFile[i]);
                if (!m.Success)
                {
                    continue;
                }
                int id = int.Parse(m.Groups["id"].Value);

                StringBuilder body = new();
                body.Append(m.Groups["body"].Value);

                int bracketNum =
                    stepFile[i].Count(x => x == '(') -
                    stepFile[i].Count(x => x == ')');
                while (bracketNum > 0)
                {
                    i++;
                    body.Append(stepFile[i]);
                    bracketNum +=
                    stepFile[i].Count(x => x == '(') -
                    stepFile[i].Count(x => x == ')');
                }
                data.Add(new StepEntity(id, body.ToString()));
            }

            return new StepRepresentation(header.ToString(), data);
        }
    }
}
