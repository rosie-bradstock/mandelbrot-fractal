// massive credit to nicsure: https://www.youtube.com/watch?v=mBg74yR3ZiY

// for some reason this has to be run using dotnet run instead of f5

using System.Runtime.InteropServices;
using System;
using System.Drawing;
using System.Diagnostics;

public class Complex // class to deal with complex numbers
{
    public double a; // real part
    public double b; // imaginary part

    public Complex(double a, double b) // constructor for complex number with real part a and imaginary part b (form a + bi)
    {
        this.a = a;
        this.b = b;
    }

    public void Square() // squares the complex number by using bracket expansion -> (a+bi)^2 = a^2 + (bi)^2 + 2abi -> a^2 - b^2 + 2abi
    {
        double temp = (a * a) - (b * b);
        b = 2.0 * a * b;
        a = temp;
    }

    public double Magnitude() // finds the magnitude of the complex number using pythag -> sqrt(a^2 + b^2)
    {
        return Math.Sqrt(a * a + b * b);
    }

    public void Add(Complex c) // adds another complex number by simply adding the real and imaginary parts together
    {
        a += c.a;
        b += c.b;
    }
}

class GenerateFractal // class to generate the image
{
    static void Main()

    {
        int width = 10000, height = 10000; // Set the image size

        Bitmap bitmap = new Bitmap(width, height); // Create a new image with a white background

        Stopwatch stopwatch = new Stopwatch(); // create a stopwatch
        stopwatch.Start(); // start a stopwatch

        for (int x = 0; x < width; x++) // loop through the x coordinates of the image
        {
            for (int y = 0; y < height; y++) // loops through the y coordinates of the image
            {

                // the number that width and height are divided by can be adjusted to zoom in or out on different regions of the fractal
                double a = (double) (x - (width / 2)) / (double)(width / 4); // normalises x to the complex plane
                double b = (double) (y - (height / 2)) / (double)(height / 4); // normalises y to the complex plane

                Complex c = new Complex(a, b); // creates a new complex number with real part a and imaginary part b
                Complex z = new Complex(0, 0); // creates a new complex number with real part 0 and imaginary part 0

                int i = 0;
                int iterations = 1024;

                do // loops for the number of iterations
                {
                    // follows the formula zn+1 = zn^2 + c, z0 = 0
                    // numbers that become greater than 2 (radius of the fractal) before reaching the maximum number of iterations are not part of the set
                    // numbers that do not become greater than 2 are part of the set

                    i++;
                    z.Square();
                    z.Add(c);
                    if (z.Magnitude() > 2.0) break;
                }
                while (i < iterations);

                Color outsideColour = Color.FromArgb(i % 255, 0, i % 255); // calculates the colour using a simple gradient outside the fractal based on the number of iterations
                Color insideColour = Color.Black;
                bitmap.SetPixel(x, y, i<iterations?outsideColour:insideColour); // Set the pixel color
            }
        }

        stopwatch.Stop(); // stop the stopwatch
        TimeSpan elapsedTime = stopwatch.Elapsed; // find the elapsed time

        bitmap.Save("mandelbrot_fractal_image.png", System.Drawing.Imaging.ImageFormat.Png); // Save the image

        Console.WriteLine($"Mandelbrot Fractal with resolution {width} x {height} rendered in {elapsedTime.TotalSeconds:F2} seconds");

    }
}