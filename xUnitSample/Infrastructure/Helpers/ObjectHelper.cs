namespace xUnitSample.Infrastructure.Helpers;

public class ObjectHelper
{
    public static bool Compare(object obj, object another)
    {
        if (ReferenceEquals(obj, another)) return true;
        if ((obj == null) || (another == null)) return false;
        if (obj.GetType() != another.GetType()) return false;

        //properties: int, double, DateTime, etc, not class
        if (!obj.GetType().IsClass) return obj.Equals(another);

        var result = true;
        foreach (var property in obj.GetType().GetProperties())
        {
            var objValue = property.GetValue(obj);
            var anotherValue = property.GetValue(another);
            //Recursion
            if (!DeepCompare(objValue, anotherValue)) result = false;
        }

        return result;
    }
        
    private static bool DeepCompare(object obj, object another)
    {
        if (ReferenceEquals(obj, another)) return true;
        if ((obj == null) || (another == null)) return false;
        //Compare two object's class, return false if they are difference
        if (obj.GetType() != another.GetType()) return false;

        var result = true;
        //Get all properties of obj
        //And compare each other
        foreach (var property in obj.GetType().GetProperties())
        {
            var objValue = property.GetValue(obj);
            var anotherValue = property.GetValue(another);
            if (!objValue.Equals(anotherValue)) result = false;
        }

        return result;
    }
}