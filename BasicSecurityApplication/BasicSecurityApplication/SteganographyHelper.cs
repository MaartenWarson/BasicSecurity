using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicSecurityApplication
{
    class SteganographyHelper
    {
        public enum State
        {
            Hiding,
            Filling_With_Zeros
        };

        public static Bitmap EmbedText(string text, Bitmap bmp)
        {
            //We gaan eigenlijk karakters in de image verbergen
            State state = State.Hiding;

            int charIndex = 0; //Houdt de index bij van het karakter dat verborgen wordt
            int charValue = 0; //Houdt de waarde van het karakter bij dat geconverted is naar een int
            long pixelElementIndex = 0; //Houdt de index van de het color-element (R, G of B) dat op dit moment verwerkt wordt bij
            int zeros = 0; //Houdt het aantal 0'en bij dat zijn toegevoegd op het einde van het proces
            int R = 0, G = 0, B = 0; // houdt pixel-elementen bij

            //Gaat door de rijen (pixels)
            for (int i = 0; i < bmp.Height; i++)
            {
                //Gaat door de kolommen (pixels)
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i); //Pixel die momenteel wordt bijgehouden

                    //De least significant bit (LSB) wordt verwijderd in het pixel-element
                    R = pixel.R - pixel.R % 2;
                    G = pixel.G - pixel.G % 2;
                    B = pixel.B - pixel.B % 2;

                    //Iedere pixel gaat door het element (RGB)
                    for (int n = 0; n < 3; n++)
                    {
                        //Controleer of de nieuwe 8 bits processed zijn
                        if (pixelElementIndex % 8 == 0)
                        {
                            //Controle of het proces beïndigd is => wanneer 8 nullen zijn toegevoegd is het gedaan
                            if (state == State.Filling_With_Zeros && zeros == 8)
                            {
                                //Maakt de laatste pixel in orde (ookal zijn niet alle pixels aan bod gekomen)
                                if ((pixelElementIndex - 1) % 3 < 2)
                                {
                                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }

                                //Returnt de bitmap met de tekst in verborgen
                                return bmp;
                            }

                            //Controleert of alle karakters verborgen zijn
                            if (charIndex >= text.Length)
                            {
                                //Start met het toevoegen van nullen om het einde van de tekst aan te geven
                                state = State.Filling_With_Zeros;
                            }
                            else
                            {
                                //Gaat naar het volgende karakter en doet het proces opnieuw
                                charValue = text[charIndex++];
                            }
                        }

                        //Controleert welk pixel-element aan de beurt is om een bit in zijn LSB te verbergen
                        switch (pixelElementIndex % 3)
                        {
                            case 0:
                                {
                                    if (state == State.Hiding)
                                    {
                                        //Het meest rechtste bit in het karakter zal zijn charValue%2
                                        //Om deze waarde in de plaats van de LSB van het pixelelement te plaatsen, gewoon eraan toevoegen
                                        //Denk eraan dat de LSB van het pixel-element gewist was voor dit proces
                                        R += charValue % 2;

                                        //Verwijdert het toegevoegde meest rechtse bit van het karakter zodat we volgende keer de volgende kunnen bereiken
                                        charValue /= 2;
                                    }
                                }
                                break;
                            case 1:
                                {
                                    if (state == State.Hiding)
                                    {
                                        G += charValue % 2;

                                        charValue /= 2;
                                    }
                                }
                                break;
                            case 2:
                                {
                                    if (state == State.Hiding)
                                    {
                                        B += charValue % 2;

                                        charValue /= 2;
                                    }

                                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }
                                break;
                        }

                        pixelElementIndex++;

                        if (state == State.Filling_With_Zeros)
                        {
                            //Verhogen van het aantal nullen totdat we 8 nullen bereikt hebben
                            zeros++;
                        }
                    }
                }
            }

            return bmp; //returnt nieuwe bitmap (image)
        }

        public static string ExtractText(Bitmap bmp)
        {
            int colorUnitIndex = 0;
            int charValue = 0;

            //Houdt de tekst bij die gedecordeerd zal worden
            string extractedText = String.Empty;

            //Gaat door de rijen
            for (int i = 0; i < bmp.Height; i++)
            {
                //Gaat door de kolommen
                for (int j = 0; j < bmp.Width; j++)
                {
                    Color pixel = bmp.GetPixel(j, i);

                    //Gaat door ieder pixel-element (RGB)
                    for (int n = 0; n < 3; n++)
                    {
                        switch (colorUnitIndex % 3)
                        {
                            case 0:
                                {
                                    //Pakt de LSB van het pixel-element (=> pixel.R % 2)
                                    //Voegt een bit aan de rechterkant van het huidig karakter toe
                                    //=> charValue = charValue * 2
                                    //Vervangt de toegevoegde bit (standaard waarde 0) met de LSB van het pixel-element door optelling
                                    charValue = charValue * 2 + pixel.R % 2;
                                }
                                break;
                            case 1:
                                {
                                    charValue = charValue * 2 + pixel.G % 2;
                                }
                                break;
                            case 2:
                                {
                                    charValue = charValue * 2 + pixel.B % 2;
                                }
                                break;
                        }

                        colorUnitIndex++;

                        //Als 8 bits zijn toegevoegd, wordt het huidige karakter bij de tekst toegevoegd
                        if (colorUnitIndex % 8 == 0)
                        {
                            //Reversen want alles gebeurt telkens aan de rechterkant
                            charValue = reverseBits(charValue);

                            //Is alleen 0 als het het stopkarakter is (8 bits)
                            if (charValue == 0)
                            {
                                return extractedText;
                            }

                            //Convert van int naar char
                            char c = (char)charValue;

                            //Voegt het huidige karakter toe aan de tekst
                            extractedText += c.ToString();
                        }
                    }
                }
            }

            return extractedText;
        }

        public static int reverseBits(int n)
        {
            int result = 0;

            for (int i = 0; i < 8; i++)
            {
                result = result * 2 + n % 2;

                n /= 2;
            }

            return result;
        }
    }
}
