// The following program compares 2 methods for determining whether a set of planar points is in 'general position' i.e. whether any 3 of them are collinear
// The first method is a brute force iteration over triples, which is O(n**3)
// The second uses projective duality to convert the points to lines and checks whether any 3 of these lines meet in a single point. It is O(n**2)

using System;
using System.Collections.Generic;

class Program {
  public static void Main (string[] args) {
    // Brute force algorithm
    // Returns a bool telling us whether points are in general position
    static bool bruteForce(List<(double, double)> points) {
      for (int i = 0; i < points.Count; i++) {
        for (int j = i+1; j < points.Count; j++) {
          for (int k = j+1; k < points.Count; k++) {
            // Get coordinates for our triple
            var x1 = points[i].Item1;
            var y1 = points[i].Item2;

            var x2 = points[j].Item1;
            var y2 = points[j].Item2;

            var x3 = points[k].Item1;
            var y3 = points[k].Item2;

            // These are the gradients between our 2 pairs of points in the triple
            var gradient1 = (y2-y1)/(x2-x1);
            var gradient2 = (y3-y2)/(x3-x2);

            // Check whether the points are collinear
            if (gradient1 == gradient2) {
              return false;
            }
          }
        }
      }

      return true;
    }

    // Duality algorithm
    // Returns a bool telling us whether points are in general position
    static bool duals(List<(double, double)> points) {
      // List to store the intersections of the duals
      var intersections = new List<(double,double)>();
      
      // Iterate over pairs of points
      for (int i = 0; i < points.Count; i++) {
        for (int j = i+1; j < points.Count; j++) {
          var point1 = points[i];
          var point2 = points[j];

          // These are the gradient (m) and y intercepts (c) for the duals to the points
          var m1 = point1.Item1;
          var c1 = -point1.Item2;
          var m2 = point2.Item1;
          var c2 = -point2.Item2;

          // Find intersection of duals
          var intersectionx = (c2-c1)/(m1-m2);
          var intersectiony = m1*intersectionx + c1;

          intersections.Add((intersectionx, intersectiony));
        }
      }

      // To check whether there are duplicates in our intersection list, convert intersections list to HashSet in and compare sizes
      // This operation is O(n**2) because the length of our intersection list is O(n**2) and the conversion is linear in the length of the list
      var intersectionSet = new HashSet<(double,double)>(intersections);

      return !(intersections.Count > intersectionSet.Count);
      
    }

    // Try the methods for some collinear points
    // We expect both methods to return false
    var collinear = new List<(double, double)>();
    collinear.Add((5.5344, 4.243));
    collinear.Add((1, 2));
    collinear.Add((2, 4));
    collinear.Add((3, 6));

    Console.WriteLine("Collinear set is in general position?");
    Console.WriteLine("Brute force: "+bruteForce(collinear));
    Console.WriteLine("Duals: "+duals(collinear));

    // Generate list of random points
    var randomPoints = new List<(double,double)>();
    Random rnd = new Random();
    for (int i=0; i<200; i++) {
      randomPoints.Add((rnd.NextDouble(), rnd.NextDouble()));
    }

    // Time the execution of both methods
    var watch1 = new System.Diagnostics.Stopwatch();
    watch1.Start();
    var brute = bruteForce(randomPoints);
    watch1.Stop();

    var watch2 = new System.Diagnostics.Stopwatch();
    watch2.Start();
    var dual = duals(randomPoints);
    watch2.Stop();

    Console.WriteLine("Large set is in general position?");
    Console.WriteLine($"Brute force: {brute} in {watch1.ElapsedMilliseconds} ms");
    Console.WriteLine($"Duals: {dual} in {watch2.ElapsedMilliseconds} ms");
    
  }
}