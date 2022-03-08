using System.Collections.Generic;

namespace WikipediaDeathsPages.Service.Helpers
{
    public static class Diminutives
    {
        public static List<string> GetNames(string givenName)
        {
            // Diminutives of given name, most common first.
            switch (givenName)
            {
                case "Abigail":
                    return new List<string> { "Abby", "Nabby" };
                case "Abraham":
                case "Abram":
                    return new List<string> { "Abe", "Aby" };
                case "Agnes":
                    return new List<string> { "Aggie", "Aggy" };
                case "Albert":
                    return new List<string> { "Bert", "Bertie" };
                case "Alexander":
                    return new List<string> { "Alec", "Alex", "Lex", "Sandy" };
                case "Alfred":
                    return new List<string> { "Alf", "Fred", "Freddy", "Alfie" };
                case "Alice":
                case "Alicia":
                    return new List<string> { "Ally", "Allie", "Elsie" };
                case "Andrew":
                    return new List<string> { "Andy" };
                case "Ann":
                case "Anna":
                case "Anne":
                case "Annette":
                    return new List<string> { "Annie" };
                case "Anthony":
                case "Antony":
                    return new List<string> { "Tony", "Tone" };
                case "Antoinette":
                    return new List<string> { "Netty", "Net" };
                case "Arabella":
                    return new List<string> { "Bella", "Bel", "Belle" };
                case "Archibald":
                    return new List<string> { "Archie", "Archy", "Baldie" };
                case "Augustus":
                    return new List<string> { "Gus", "Gussie", "Gustus" };
                case "Barbara":
                    return new List<string> { "Bab", "Babs", "Babbie" };
                case "Bartholomew":
                    return new List<string> { "Bart", "Bat" };
                case "Benedict":
                    return new List<string> { "Bennet" };
                case "Benjamin":
                    return new List<string> { "Ben", "Benny" };
                case "Bernard":
                    return new List<string> { "Barney" };
                case "Bertha":
                case "Bertram":
                    return new List<string> { "Bertie", "Berty", "Bert" };
                case "Burton":
                    return new List<string> { "Burt" };
                case "Caroline":
                    return new List<string> { "Carrie", "Caddie", "Carol" };
                case "Casimir":
                    return new List<string> { "Cassie", "Cas" };
                case "Catherine":
                case "Catherina":
                case "Katharine":
                case "Katherine":
                    return new List<string> { "Katie", "Casy", "Kate", "Kathleen", "Cathie", "Kathie", "Kit", "Kitty" };
                case "Cecilia":
                case "Cecily":
                    return new List<string> { "Cissy", "Sis", "Cis", "Cissie" };
                case "Charles":
                    return new List<string> { "Charlie", "Charley" };
                case "Christian":
                    return new List<string> { "Chris", "Christie", "Christy" };
                case "Christina":
                    return new List<string> { "Chris", "Chrissy", "Chrissie", "Tina" };
                case "Christopher":
                    return new List<string> { "Chris" };
                case "Clara":
                case "Clarice":
                case "Clarissa":
                    return new List<string> { "Clare", "Claire" };
                case "Constance":
                    return new List<string> { "Connie", "Conny" };
                case "Daniel":
                    return new List<string> { "Dan", "Danny", "Dannie" };
                case "David":
                    return new List<string> { "Dave", "Davy", "Davie" };
                case "Diana":
                    return new List<string> { "Di", "Die" };
                case "Dorothea":
                case "Dorothy":
                    return new List<string> { "Dolly", "Dol", "Dora", "Dollie" };
                case "Edmund":
                    return new List<string> { "Ed", "Ned", "Neddy" };
                case "Edward":
                    return new List<string> { "Ed", "Eddie", "Eddy", "Ned", "Neddy", "Ted", "Teddy" };
                case "Edwin":
                    return new List<string> { "Ed", "Eddie", "Eddy" };
                case "Eleanor":
                case "Elinor":
                case "Leonora":
                    return new List<string> { "Ella", "Ellen", "Nell", "Nellie", "Nora", "Norah" };
                case "Elisabeth":
                case "Eliza":
                case "Elizabeth":
                    return new List<string> { "Beth", "Betsy", "Bess", "Bessie", "Bessy", "Betty", "Elsie", "Lisa", "Liza" };
                case "Elsbeth":
                case "Elspeth":
                    return new List<string> { "Elspie", "Elsie" };
                case "Emeline":
                case "Emmeline":
                case "Emily":
                case "Emma":
                    return new List<string> { "Emmy", "Emmie", "Emm" };
                case "Esther":
                case "Hester":
                case "Hesther":
                    return new List<string> { "Essie", "Essy" };
                case "Eugenia":
                    return new List<string> { "Genie" };
                case "Euphemia":
                    return new List<string> { "Effie", "Euphie", "Phemie", "Phamie", "Effy" };
                case "Evelina":
                case "Eveline":
                case "Evelyn":
                    return new List<string> { "Eva", "Eve" };
                case "Ezekial":
                    return new List<string> { "Zeke" };
                case "Florence":
                    return new List<string> { "Flo", "Flossie" };
                case "Frances":
                    return new List<string> { "Fanny", "Fannie" };
                case "Francis":
                    return new List<string> { "Frank", "Francie", "Frankie", "Franky" };
                case "Frederic":
                case "Frederick":
                case "Frederica":
                case "Frederico":
                    return new List<string> { "Fred", "Freddie", "Freddy" };
                case "Gabriel":
                    return new List<string> { "Gabe", "Gaby", "Gabby" };
                case "George":
                    return new List<string> { "Dod", "Doddy", "Geordie", "Georgie" };
                case "Georgina":
                case "Georgiana":
                    return new List<string> { "Georgie", "Georgy" };
                case "Gertrude":
                    return new List<string> { "Gertie", "Gerty", "Trudy", "Trudie" };
                case "Gilbert":
                    return new List<string> { "Gil" };
                case "Helen":
                case "Helena":
                    return new List<string> { "Nell", "Nellie", "Lena" };
                case "Henrietta":
                    return new List<string> { "Etta", "Hetty", "Nettie", "Netty" };
                case "Harold":
                case "Henry":
                    return new List<string> { "Harry", "Harrie", "Hal", "Hen", "Henny" };
                case "Hugh":
                case "Hugo":
                    return new List<string> { "Hughie" };
                case "Humphrey":
                case "Humphry":
                    return new List<string> { "Humph" };
                case "Isaac":
                case "Izaak":
                    return new List<string> { "Ike", "Ik" };
                case "James":
                case "Jacob":
                case "Jacov":
                    return new List<string> { "Jim", "Jimmy", "Jemmy", "Jimmie", "Jem" };
                case "Jacomina":
                    return new List<string> { "Mien" };
                case "Jane":
                    return new List<string> { "Janet", "Jean", "Jeanie", "Jeannie", "Jeany" };
                case "Janet":
                case "Jennifer":
                    return new List<string> { "Jenny", "Jennie", "Jen" };
                case "Jessie":
                    return new List<string> { "Jess" };
                case "John":
                    return new List<string> { "Johnnie", "Johnny", "Jack", "Jock" };
                case "Joseph":
                    return new List<string> { "Joe", "Joey" };
                case "Josephine":
                    return new List<string> { "Jo", "Josy", "Josie", "Jozy", "Pheny" };
                case "Joshua":
                    return new List<string> { "Josh" };
                case "Judith":
                    return new List<string> { "Judy", "Judie" };
                case "Julian":
                case "Julius":
                    return new List<string> { "Jule" };
                case "Kathleen":
                    return new List<string> { "Kathy", "Katie", "Kath" };
                case "Kenneth":
                    return new List<string> { "Ken" };
                case "Laurence":
                case "Lawrence":
                case "Lorenzo":
                    return new List<string> { "Larry", "Larrie" };
                case "Leonard":
                    return new List<string> { "Lenny", "Len", "Lennie" };
                case "Letitia":
                case "Lettice":
                    return new List<string> { "Letty", "Lettie" };
                case "Lewis":
                case "Louis":
                case "Ludovic":
                    return new List<string> { "Lou", "Lewie", "Lew", "Louie" };
                case "Louisa":
                case "Louise":
                    return new List<string> { "Lou", "Louie" };
                case "Madeline":
                    return new List<string> { "Maud", "Maudlin" };
                case "Magdalene":
                    return new List<string> { "Lena", "Maud" };
                case "Margaret":
                    return new List<string> { "Margie", "Margy", "Marjory", "Marjorie", "Madge", "Mag", "Maggie", "Meg", "Meggy", "Peg", "Peggy" };
                case "Marion":
                    return new List<string> { "Mamie" };
                case "Martha":
                    return new List<string> { "Mat", "Matty", "Pat", "Patty", "Patti" };
                case "Mary":
                case "Miriam":
                    return new List<string> { "May", "Moll", "Molly", "Mollie" };
                case "Matilda":
                case "Mathilda":
                    return new List<string> { "Mat", "Mattie", "Matty", "Maud", "Tilda", "Tillie" };
                case "Matthew":
                case "Matthias":
                    return new List<string> { "Matt", "Mat" };
                case "Michael":
                    return new List<string> { "Mike", "Micky", "Mickey" };
                case "Moses":
                    return new List<string> { "Mose", "Mosey" };
                case "Nancy":
                    return new List<string> { "Nan", "Nance", "Nina" };
                case "Nicholas":
                case "Nicolas":
                    return new List<string> { "Nick", "Nic", "Nicky" };
                case "Oliver":
                    return new List<string> { "Ollie", "Olly", "Nol", "Nolly" };
                case "Patrick":
                    return new List<string> { "Pat", "Paddy" };
                case "Patricia":
                    return new List<string> { "Pat", "Patty" };
                case "Peter":
                    return new List<string> { "Pete" };
                case "Philip":
                    return new List<string> { "Phil", "Pip" };
                case "Philippa":
                    return new List<string> { "Pippa" };
                case "Prudence":
                    return new List<string> { "Prue", "Prudy" };
                case "Raymond":
                case "Raymund":
                    return new List<string> { "Ray" };
                case "Rebecca":
                case "Rebekah":
                    return new List<string> { "Beck", "Bex", "Becky", "Beckie" };
                case "Reginald":
                case "Reynold":
                    return new List<string> { "Reg", "Reggie", "Reggy" };
                case "Richard":
                    return new List<string> { "Dick", "Dickie", "Dicky", "Rick", "Ricky", "Rickie" };
                case "Robert":
                case "Rupert":
                    return new List<string> { "Bob", "Bobby", "Bobbie", "Rob", "Robby", "Robbie", "Robin" };
                case "Roderick":
                case "Roderic":
                    return new List<string> { "Rick", "Ricky", "Rickie", "Rod", "Roddie", "Roddy" };
                case "Roger":
                    return new List<string> { "Rodge", "Hodge", "Hodgekin" };
                case "Rosa":
                case "Rosabella":
                case "Rosalia":
                case "Rosalie":
                case "Rosalind":
                    return new List<string> { "Rosie" };
                case "Samuel":
                    return new List<string> { "Sam", "Sammy" };
                case "Sarah, Sara":
                    return new List<string> { "Sal", "Sally" };
                case "Sebastian":
                    return new List<string> { "Seb" };
                case "Silvester":
                case "Sylvester":
                    return new List<string> { "Vester", "Vest", "Sil", "Syl" };
                case "Sophia":
                    return new List<string> { "Sophie", "Sophy" };
                case "Stephen":
                    return new List<string> { "Steve", "Stevie", "Steenie" };
                case "Susan":
                case "Susanna":
                case "Susannah":
                    return new List<string> { "Sue", "Suke", "Suky", "Susie", "Susy" };
                case "Theodora":
                    return new List<string> { "Dora" };
                case "Theresa":
                    return new List<string> { "Terry", "Tracie", "Tracy" };
                case "Thomas":
                    return new List<string> { "Tom", "Tommy", "Tam", "Tammie" };
                case "Timothy":
                    return new List<string> { "Tim" };
                case "Tobias":
                    return new List<string> { "Toby" };
                case "Victor":
                    return new List<string> { "Vic" };
                case "Vincent":
                    return new List<string> { "Vince", "Vin", "Vinty" };
                case "Walter":
                    return new List<string> { "Wat", "Watty" };
                case "Willem":
                    return new List<string> { "Wim", "Willie" };
                case "William":
                    return new List<string> { "Will", "Willie", "Willy", "Bill", "Billie", "Billy" };
                case "Winifred":
                case "Winfred":
                    return new List<string> { "Winnie", "Winny" };
                case "Zachariah":
                case "Zechariah":
                    return new List<string> { "Zach", "Zacky", "Zackie", "Zak" };
                default:
                    return new List<string>();
            }
        }
    }
}
