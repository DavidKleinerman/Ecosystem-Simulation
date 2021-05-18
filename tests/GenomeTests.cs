using Godot;
using System;

public class GenomeTests : WAT.Test
{
    public override String Title()
	{
		return "Genome Unit Tests";
	}
    
    private bool CheckChromosome(Godot.Collections.Array Chromosome){
        bool flag = true;
        for(int i=0; i< Chromosome.Count; i++){
            if((float)Chromosome[i] > 100 || (float)Chromosome[i]<0){
                flag = false;
            }
        }
        return flag;
    }

    [Test]
    public void ArtificialCombineLowestValueZeroVariationTest(){
        const float V = 0;
        const int W = 0;
        const int X = 15;
        const int Y = 15;
        bool Z = true;
        Godot.Collections.Array initialValues = new Godot.Collections.Array();
        Genome gen = new Genome();
        for(int i=0; i<X; i++){
            initialValues.Add(V);
        }
        gen.ArtificialCombine(initialValues,W);
        if(CheckChromosome(gen.getMaternal()) && CheckChromosome(gen.getPaternal())){
            Z = true;
        }
        else{
            Z = false;
        }
        Assert.IsEqual(X,gen.getMaternal().Count,"Then it Passes");
        Assert.IsEqual(Y,gen.getPaternal().Count,"Then it Passes");
        Assert.IsTrue(Z,"Then it Passes");
        
    }

     [Test]
    public void ArtificialCombineMiddleValueZeroVariationTest(){
        const float V = 50;
        const int W = 0;
        const int X = 15;
        const int Y = 15;
        bool Z = true;
        Godot.Collections.Array initialValues = new Godot.Collections.Array();
        Genome gen = new Genome();
        for(int i=0; i<X; i++){
            initialValues.Add(V);
        }
        gen.ArtificialCombine(initialValues,W);
        if(CheckChromosome(gen.getMaternal()) && CheckChromosome(gen.getPaternal())){
            Z = true;
        }
        else{
            Z = false;
        }
        Assert.IsEqual(X,gen.getMaternal().Count,"Then it Passes");
        Assert.IsEqual(Y,gen.getPaternal().Count,"Then it Passes");
        Assert.IsTrue(Z,"Then it Passes");
        
    }

      [Test]
    public void ArtificialCombineHighestValueZeroVariationTest(){
        const float V = 100;
        const int W = 0;
        const int X = 15;
        const int Y = 15;
        bool Z = true;
        Godot.Collections.Array initialValues = new Godot.Collections.Array();
        Genome gen = new Genome();
        for(int i=0; i<X; i++){
            initialValues.Add(V);
        }
        gen.ArtificialCombine(initialValues,W);
        if(CheckChromosome(gen.getMaternal()) && CheckChromosome(gen.getPaternal())){
            Z = true;
        }
        else{
            Z = false;
        }
        Assert.IsEqual(X,gen.getMaternal().Count,"Then it Passes");
        Assert.IsEqual(Y,gen.getPaternal().Count,"Then it Passes");
        Assert.IsTrue(Z,"Then it Passes");
        
    }

     [Test]
    public void ArtificialCombineLowestValueNotZeroVariationTest(){
        const float V = 0;
        const int W = 20;
        const int X = 15;
        const int Y = 15;
        bool Z = true;
        Godot.Collections.Array initialValues = new Godot.Collections.Array();
        Genome gen = new Genome();
        for(int i=0; i<X; i++){
            initialValues.Add(V);
        }
        gen.ArtificialCombine(initialValues,W);
        if(CheckChromosome(gen.getMaternal()) && CheckChromosome(gen.getPaternal())){
            Z = true;
        }
        else{
            Z = false;
        }
        Assert.IsEqual(X,gen.getMaternal().Count,"Then it Passes");
        Assert.IsEqual(Y,gen.getPaternal().Count,"Then it Passes");
        Assert.IsTrue(Z,"Then it Passes");
        
    }
     [Test]
       public void ArtificialCombineMiddleValueNotZeroVariationTest(){
        const float V = 50;
        const int W = 20;
        const int X = 15;
        const int Y = 15;
        bool Z = true;
        Godot.Collections.Array initialValues = new Godot.Collections.Array();
        Genome gen = new Genome();
        for(int i=0; i<X; i++){
            initialValues.Add(V);
        }
        gen.ArtificialCombine(initialValues,W);
        if(CheckChromosome(gen.getMaternal()) && CheckChromosome(gen.getPaternal())){
            Z = true;
        }
        else{
            Z = false;
        }
        Assert.IsEqual(X,gen.getMaternal().Count,"Then it Passes");
        Assert.IsEqual(Y,gen.getPaternal().Count,"Then it Passes");
        Assert.IsTrue(Z,"Then it Passes");
        
    }

       [Test]
       public void ArtificialCombineHighestValueNotZeroVariationTest(){
        const float V = 100;
        const int W = 20;
        const int X = 15;
        const int Y = 15;
        bool Z = true;
        Godot.Collections.Array initialValues = new Godot.Collections.Array();
        Genome gen = new Genome();
        for(int i=0; i<X; i++){
            initialValues.Add(V);
        }
        gen.ArtificialCombine(initialValues,W);
        if(CheckChromosome(gen.getMaternal()) && CheckChromosome(gen.getPaternal())){
            Z = true;
        }
        else{
            Z = false;
        }
        Assert.IsEqual(X,gen.getMaternal().Count,"Then it Passes");
        Assert.IsEqual(Y,gen.getPaternal().Count,"Then it Passes");
        Assert.IsTrue(Z,"Then it Passes");
        
    }


     private bool CheckDominanceMask(Godot.Collections.Array dominanceMask){
        bool flag = true;
        for(int i=0; i< dominanceMask.Count; i++){
            if(!((int)dominanceMask[i] == 1 || (int)dominanceMask[i] == 0)){
                flag = false;
            }
        }
        return flag;
    }
       [Test]
       public void RecombinationMinLengthTest(){
        const int X = 0;
        bool Y = true;
        Godot.Collections.Array maternal = new Godot.Collections.Array();
        Godot.Collections.Array paternal = new Godot.Collections.Array();
        Genome gen = new Genome();
        for(int i=0; i<X; i++){
            maternal.Add(X);
            paternal.Add(X);
        }
        gen.Recombination(maternal,paternal);
        Y = CheckDominanceMask(gen.getDominanceMask());
        Assert.IsEqual(X,gen.getDominanceMask().Count,"Then it Passes");
        Assert.IsTrue(Y,"Then it Passes");
    }

     [Test]
       public void RecombinationMiddleLengthTest(){
        const int X = 10;
        bool Y = true;
        Godot.Collections.Array maternal = new Godot.Collections.Array();
        Godot.Collections.Array paternal = new Godot.Collections.Array();
        Genome gen = new Genome();
        for(int i=0; i<X; i++){
            maternal.Add(X);
            paternal.Add(X);
        }
        gen.Recombination(maternal,paternal);
        Y = CheckDominanceMask(gen.getDominanceMask());
        Assert.IsEqual(X,gen.getDominanceMask().Count,"Then it Passes");
        Assert.IsTrue(Y,"Then it Passes");
    }

      [Test]
       public void RecombinationMaxLengthTest(){
        const int X = 15;
        bool Y = true;
        Godot.Collections.Array maternal = new Godot.Collections.Array();
        Godot.Collections.Array paternal = new Godot.Collections.Array();
        Genome gen = new Genome();
        for(int i=0; i<X; i++){
            maternal.Add(X);
            paternal.Add(X);
        }
        gen.Recombination(maternal,paternal);
        Y = CheckDominanceMask(gen.getDominanceMask());
        Assert.IsEqual(X,gen.getDominanceMask().Count,"Then it Passes");
        Assert.IsTrue(Y,"Then it Passes");
    }
}
