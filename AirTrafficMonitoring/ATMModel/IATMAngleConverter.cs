using System.Security.Cryptography.X509Certificates;

namespace ATMModel
{
    public interface IATMAngleConverter
    {
        double Convert(IATMCoordinate oldCoordinate, IATMCoordinate newCoordinate);
    }
}