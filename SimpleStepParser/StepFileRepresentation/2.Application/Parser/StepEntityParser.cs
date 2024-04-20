﻿using SimpleStepParser.StepFileRepresentation._1.Domain.Entities;
using System.Text.RegularExpressions;

namespace SimpleStepParser.StepFileRepresentation._2.Application.Parser
{
    internal class StepEntityParser
    {
        private static readonly Regex _stepDirectionRegex
            = new Regex(@"^DIRECTION\('(?<name>.*)',\((?<i>\d*.\d*),(?<j>\d*.\d*),(?<k>\d*.\d*)\)\);");

        internal static StepDirection? TryParseToStepDirection(UndefinedStepEntity from)
        {
            if (from.Body == null)
            {
                return null;
            }

            var match = _stepDirectionRegex.Match(from.Body);
            if (!match.Success)
            {
                return null;
            }

            StepDirection result = new StepDirection()
            {
                Id = from.Id,
                Name = match.Groups["name"].Value ?? string.Empty,
                I = float.Parse(match.Groups["i"].Value),
                J = float.Parse(match.Groups["j"].Value),
                K = float.Parse(match.Groups["k"].Value),
            };

            return result;
        }

        private static readonly Regex _stepCartesianPoint
            = new Regex(@"^CARTESIAN_POINT\('(?<name>.*)',\((?<x>\d*.\d*),(?<y>\d*.\d*),(?<z>\d*.\d*)\)\);");
        //For more information: https://www.steptools.com/docs/stp_aim/html/t_cartesian_point.html
        internal static StepCartesianPoint? TryParseToStepCartesianPoint(UndefinedStepEntity from)
        {
            if (from.Body == null)
            {
                return null;
            }

            var match = _stepCartesianPoint.Match(from.Body);
            if (!match.Success)
            {
                return null;
            }

            StepCartesianPoint result = new StepCartesianPoint()
            {
                Id = from.Id,
                Name = match.Groups["name"].Value ?? string.Empty,
                X = float.Parse(match.Groups["x"].Value),
                Y = float.Parse(match.Groups["y"].Value),
                Z = float.Parse(match.Groups["z"].Value),
            };

            return result;
        }

        private static readonly Regex _stepAxis2Placement3D
            = new Regex(@"^AXIS2_PLACEMENT_3D\('(?<name>.*)',#(?<point>\d*),#(?<zAxis>\d*),#(?<xAxis>\d*)\);");
        //For more information^ https://www.steptools.com/stds/stp_aim/html/t_axis2_placement_3d.html
        internal static StepAxis2Placement3D? TryParseToStepAxis2Placement3D(UndefinedStepEntity from)
        {
            if (from.Body == null)
            {
                return null;
            }

            var match = _stepAxis2Placement3D.Match(from.Body);
            if (!match.Success)
            {
                return null;
            }

            StepAxis2Placement3D result = new StepAxis2Placement3D()
            {
                Id = from.Id,
                Name = match.Groups["name"].Value ?? string.Empty,
                LocationPointId = int.Parse(match.Groups["point"].Value),
                ZAxisId = int.Parse(match.Groups["zAxis"].Value),
                XAxisId = int.Parse(match.Groups["xAxis"].Value),
            };

            return result;
        }

    }
}
