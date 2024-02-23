using System.Diagnostics;
using VGE;
using VGE.Graphics.Scenes;

namespace Battlezone
{
	public static class PositionCalculationTools
	{
		/// <summary>
		/// Kalkulacja następnej pozycji kiedy chcielibiśmy kierować sie do gracza
		/// </summary>
		/// <returns>Pierwsze pozycja, drugie rotacja w jaką obiekt powinien sie obróć</returns>
		public static (Point, float) NextPositionTowardsPlayer(Transform transform, float speed, float delta)
		{
			/* 
             * dlaczego to nie jest w silniku?
             * bo nie miałem czasu zaimplementowac tego porządnie.
             * nie będę się niekulturalnie wyrażać.
             * 
             * SZUKAŁEM WSZĘDZIE PARE DNI JAK ZROBIĆ OBRÓT PUNKTU 3D DO PUNKTU 3D ALE NIKT NIE
             * NAPISAŁ TEGO NA TYLE PORZĄDNIE ABY DAŁO SIĘ TO ZROZUMIEĆ TO JEST ABSOLUTNA TRAGEDIA
             * JA SIĘ PO PROSTU PODDAŁEM.
             */

			//angle
			Point offset = transform.Position - Scene3D.Camera.Position;

			float c = MathF.Sqrt(MathF.Pow(offset.X, 2) + MathF.Pow(offset.Z, 2));

			float sinAngleToPlayer = offset.Z / c;
			float angleToPlayer = (MathF.Asin(sinAngleToPlayer) * MathTools.Rad2deg);

			float rotationDelta;

			if (offset.X > 0)
				rotationDelta = angleToPlayer - transform.Rotation.Y;
			else
				rotationDelta = ((angleToPlayer * -1f) - transform.Rotation.Y) - 180;

			//next position
			float cNext = (delta * speed);

			float z = sinAngleToPlayer * cNext;
			float x = (offset.X / c) * cNext;

			transform.Position.X -= x;
			transform.Position.Z -= z;

			return (transform.Position, rotationDelta);
		}
	}
}
