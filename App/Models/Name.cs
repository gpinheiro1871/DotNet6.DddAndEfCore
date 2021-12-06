using CSharpFunctionalExtensions;

namespace App.Models
{
    public class Name : ValueObject
    {
        public string First { get; }
        public string Last { get; }

        protected Name()
        {

        }

        private Name(string first, string last)
        {
            First = first;
            Last = last;
        }

        public static Result<Name> Create(string first, string last)
        {
            if (string.IsNullOrWhiteSpace(first))
            {
                return Result.Failure<Name>("First name should not be empty");
            }
            
            if (string.IsNullOrWhiteSpace(last))
            {
                return Result.Failure<Name>("Last name should not be empty");
            }

            first = first.Trim();
            last = last.Trim();

            if (first.Length > 200)
            {
                return Result.Failure<Name>("First name is too long");
            } 
            
            if (last.Length > 200)
            {
                return Result.Failure<Name>("Last name is too long");
            }

            return Result.Success(new Name(first, last));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return First;
            yield return Last;
        }
    }
}
