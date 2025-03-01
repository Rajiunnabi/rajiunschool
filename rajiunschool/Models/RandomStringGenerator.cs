using System;

public class RandomStringGenerator
{
    private static readonly Random random = new Random();

    public static string GenerateRandomString(int length)
    {
        // Define the characters to use in the random string
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // Create a char array to store the random string
        char[] randomString = new char[length];

        // Fill the array with random characters
        for (int i = 0; i < length; i++)
        {
            randomString[i] = chars[random.Next(chars.Length)];
        }

        // Convert the char array to a string
        return new string(randomString);
    }
}

