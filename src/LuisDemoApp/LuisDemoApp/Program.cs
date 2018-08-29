using LuisDemoApp.Quote;
using Microsoft.Cognitive.LUIS;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace LuisDemoApp
{
    class Program
    {
        static void Main(string[] args)
        {

            LuisClient luisClient = new LuisClient("", "");

            while (true)
            {
                // Ler o texto a ser reconhecido
                Console.Write("Escreva algo: ");
                string input = Console.ReadLine().Trim();

                if (input.ToLower() == "exit")
                {
                    break;
                }
                else
                {
                    if (input.Length > 0)
                    {
                        // Faz a requisição e deserializa
                        LuisResult result = luisClient.Predict(input).Result;

                        // Tomada de decisão
                        HandleUserIntent(result);
                    }
                }
            }
        }

        private static void HandleUserIntent(LuisResult luisResult)
        {
            if (luisResult.TopScoringIntent.Name == "Acao_Consultar")
            {
                GetQuoteInformation(luisResult);
            }
            else
            {
                switch (luisResult.TopScoringIntent.Name)
                {
                    case "Basica_Saudacao":
                        Console.WriteLine("Olá. Tudo bem? Posso ajudar com informações sobre ações, o que você deseja saber?");
                        break;

                    case "Basica_Despedir":
                        Console.WriteLine("Tchau! Até mais!");
                        break;

                    case "Basica_Ajuda":
                        Console.WriteLine("É bem simples, sou um assistente digital e posso te ajudar com informações sobre ativos e investimentos.");
                        Console.WriteLine("Me pergunte o que você gostaria de saber e vou fazer o meu melhor para responder!");
                        break;

                    case "Basica_Agradecer":
                        Console.WriteLine("Foi um prazer poder ajudar! Se precisar de mais alguma coisa é só falar. ;)");
                        break;

                    case "Acao_Definicao":
                        Console.WriteLine("Ações são pequenas partes de uma empresa. Ou seja, todos os que são donos de uma ação, são sócios de um pedaço da empresa.");
                        break;

                    case "Investir_ComoLucrar":
                        Console.WriteLine("Você pode ganhar de três formas. Com a alta das ações que comprar, quando a empresa distribui lucros para os acionistas (dividendos) e alugando suas ações.");
                        break;

                    case "Investir_Internet":
                        Console.WriteLine("Várias corretoras oferecem o serviço de home broker, uma ferramenta que permite a compra e a venda de ações diretamente pela internet. É rápido, transparente e muito seguro. Com este serviço, você pode acompanhar o mercado e negociar suas ações de casa, no escritório e até durante uma viagem.");
                        break;

                    case "Investir_MelhorMomento":
                        Console.WriteLine("O momento ideal é quando você estiver seguro de que entendeu a mecânica básica do mercado acionário, ou seja, é uma aplicação com horizonte de resgate de médio e longo prazos, tem bom potencial de rentabilidade e traz também riscos de flutuação no valor investido.");
                        break;

                    case "Investir_TamanhoLucro":
                        Console.WriteLine("As ações são investimentos de renda variável, ou seja, não há uma rentabilidade média predeterminada. Antes de investir seu dinheiro, lembre-se que ação é um investimento de risco e para formação de patrimônio de longo prazo. A curto prazo, assim como pode valorizar, também pode desvalorizar.");
                        break;

                    case "Investir_Vantagens":
                        Console.WriteLine("Não é preciso muito dinheiro para começar. Você recebe dividendos periodicamente, tem potencial de boa rentabilidade no longo prazo, pode comprar ou vender suas ações a qualquer momento e é possível alugar suas ações fazendo um empréstimo de ativos e ganhar um rendimento extra.");
                        break;

                    case "None":
                    default:
                        Console.WriteLine("Me desculpe, não consegui entender o que você disse. Ainda estou em fase de aprendizado, mas tente falar de maneira mais clara eu talvez eu consida entender. :D");
                        break;
                }
            }
        }

        private static void GetQuoteInformation(LuisResult luisResult)
        {
            Entity quoteEntity = luisResult.GetAllEntities().FirstOrDefault(e => e.Name == "Ativo");

            string entityValue = string.Empty;

            if (quoteEntity != null)
            {
                entityValue = quoteEntity.Value;
            }
            else
            {
                Console.WriteLine("Qual o ativo ou empresa você deseja consultar?");
                entityValue = Console.ReadLine();
            }

            QuoteResult quoteResult = FakeQuote.GetPrice(entityValue);
            Console.WriteLine($"O preço do ativo {quoteResult.Name} é {quoteResult.Value:C2}.");
        }
    }
}
