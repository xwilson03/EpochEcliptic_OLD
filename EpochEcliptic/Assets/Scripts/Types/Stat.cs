using System;

[Serializable]
public class Stat {

    public static readonly Stat zero = new (0,0);

    public float flat;
    public float multi;

    public Stat(float flat = 0f, float multi = 1f) {
        this.flat = flat;
        this.multi = multi;
    }

    public override string ToString() {
        return $"[{flat},{multi}]";
    }

    public static Stat operator +(Stat lhs, Stat rhs){
        return new Stat {
            flat  = lhs.flat  + rhs.flat,
            multi = lhs.multi + rhs.multi
        };
    }

    public static Stat operator -(Stat lhs, Stat rhs){
        return new Stat {
            flat  = lhs.flat  - rhs.flat,
            multi = lhs.multi - rhs.multi
        };
    }
    
    public static Stat operator -(Stat lhs){
        return new Stat {
            flat  = lhs.flat  * -1,
            multi = lhs.multi * -1
        };
    }

    public static bool operator ==(Stat lhs, Stat rhs){
        return lhs.flat == rhs.flat
            && lhs.multi == rhs.multi;
    }

    public static bool operator !=(Stat lhs, Stat rhs){
        return lhs.flat != rhs.flat
            || lhs.multi != rhs.multi;
    }

    public override bool Equals(object obj) {
        Stat rhs = obj as Stat;
        return (rhs is not null) && (this == rhs);
    }
}