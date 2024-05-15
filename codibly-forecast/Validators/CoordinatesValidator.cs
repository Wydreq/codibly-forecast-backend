using codibly_forecast.Exceptions;

namespace codibly_forecast.Validators;

public class CoordinatesValidator
{
    public static void ValidateCoordinates(double latitude, double longitude)
    {
        if (latitude == null || longitude == null)
        {
            throw new BadRequestException("Latitude or longitude cannot be null.");
        }

        if (!(latitude is double) || !(longitude is double))
        {
            throw new BadRequestException("Latitude and longitude must be of type double.");
        }

        if (latitude < -90 || latitude > 90)
        {
            throw new BadRequestException("Latitude must be between -90 and 90 degrees.");
        }

        if (longitude < -180 || longitude > 180)
        {
            throw new BadRequestException("Longitude must be between -180 and 180 degrees.");
        }
    }
}