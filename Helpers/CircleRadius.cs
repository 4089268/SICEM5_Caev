using System;

namespace Sicem_Blazor.Helpers;

public class CircleRadius
{
    private double minIncome = 0;
    private double maxIncome = 0;
    private double minRadius = 1000;
    private double maxRadius = 50000;
    
    public CircleRadius( double minInc, double maxInc, double minR, double maxR)
    {
        this.minIncome = minInc;
        this.maxIncome = maxInc;
        this.minRadius = minR;
        this.maxRadius = maxR;
    }
    
    public double GetRadius(double income)
    {
        // Clamp income to the valid range
        income = Math.Max(minIncome, Math.Min(maxIncome, income));

        // Perform linear interpolation
        var radius = minRadius + ((income - minIncome) / (maxIncome - minIncome)) * (maxRadius - minRadius);

        return radius;
    }
}