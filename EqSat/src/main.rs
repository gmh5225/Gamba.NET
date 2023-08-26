use egg::*;

define_language! {
    pub enum Expr {
        // operations
        "**" = Pow([Id; 2]),             // (+ a b)
        "~" = Neg([Id; 1]),              // (~ a)
        "*" = Mul([Id; 2]),              // * a b)
        "+" = Add([Id; 2]),              // (+ a b)
        "&" = And([Id; 2]),              // (& a b)
        "^" = Xor([Id; 2]),              // (^ a b)
        "|" = Or([Id; 2]),               // (| a b)

        // Values:
        Variable(char),                  // (x) 
        Constant(i64),                   // (int)
    }
}

fn main() {
    println!("Hello, world!");
}
