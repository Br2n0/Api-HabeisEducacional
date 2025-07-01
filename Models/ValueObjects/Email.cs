using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Api_HabeisEducacional.Models.Common;

namespace Api_HabeisEducacional.Models.ValueObjects
{
    /// <summary>
    /// 🔄 VALUE OBJECT: Email
    /// PROPÓSITO: Encapsular e validar endereços de email de forma consistente
    /// BENEFÍCIOS:
    /// - Validação automática de formato
    /// - Normalização automática (lowercase, trim)
    /// - Imutabilidade (thread-safe)
    /// - Reutilização em todo o sistema
    /// - Encapsulamento de regras de negócio
    /// </summary>
    public class Email : ValueObject
    {
        /// <summary>
        /// Valor do email normalizado e validado
        /// Sempre em lowercase para consistência
        /// </summary>
        public string Valor { get; }
        
        /// <summary>
        /// Regex para validação de formato de email
        /// Pattern: username@domain.extension
        /// Aceita letras, números, pontos, hífens e underscores
        /// </summary>
        private static readonly Regex EmailRegex = new(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

        /// <summary>
        /// Construtor privado para garantir que emails só sejam criados via método Criar()
        /// Isso força a validação em toda criação de email
        /// </summary>
        /// <param name="valor">Email já validado e normalizado</param>
        private Email(string valor) => Valor = valor;

        /// <summary>
        /// 🏭 FACTORY METHOD: Cria um novo email com validações completas
        /// VALIDAÇÕES APLICADAS:
        /// 1. Verificação de valor nulo/vazio
        /// 2. Normalização (trim + lowercase)
        /// 3. Validação de formato via regex
        /// 4. Verificação de tamanho máximo
        /// </summary>
        /// <param name="email">String do email a ser validada</param>
        /// <returns>Instância válida de Email</returns>
        /// <exception cref="ArgumentException">Lançada quando email é inválido</exception>
        public static Email Criar(string email)
        {
            // VALIDAÇÃO 1: Verificar se não é nulo ou vazio
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email não pode ser vazio");

            // NORMALIZAÇÃO: Remove espaços e converte para minúsculo
            // Isso garante consistência no armazenamento e comparação
            var normalizado = email.Trim().ToLower();
            
            // VALIDAÇÃO 2: Verificar formato via regex
            if (!EmailRegex.IsMatch(normalizado))
                throw new ArgumentException("Email inválido");

            // VALIDAÇÃO 3: Verificar tamanho máximo (padrão RFC 5321)
            if (normalizado.Length > 256)
                throw new ArgumentException("Email muito longo");

            // Retorna instância válida
            return new Email(normalizado);
        }

        /// <summary>
        /// Implementação da comparação para Value Objects
        /// Usado pelo ValueObject base para determinar igualdade
        /// </summary>
        /// <returns>Componentes usados para comparação</returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }

        /// <summary>
        /// Retorna a representação string do email
        /// Útil para logging e exibição
        /// </summary>
        /// <returns>Valor do email</returns>
        public override string ToString() => Valor;
    }
}

/* 
═══════════════════════════════════════════════════════════════════════════════════
📋 DOCUMENTAÇÃO DO VALUE OBJECT EMAIL
═══════════════════════════════════════════════════════════════════════════════════

🎯 PROPÓSITO:
   - Garantir que todos os emails no sistema sejam válidos
   - Centralizar lógica de validação de email
   - Evitar emails duplicados por diferenças de case
   - Facilitar mudanças futuras nas regras de email

🔒 CARACTERÍSTICAS:
   - IMUTÁVEL: Uma vez criado, não pode ser alterado
   - THREAD-SAFE: Pode ser usado em ambientes multi-thread
   - VALIDADO: Impossível criar instância inválida
   - NORMALIZADO: Sempre lowercase e sem espaços extras

🏭 PADRÃO FACTORY:
   - Construtor privado impede criação direta
   - Método Criar() força validação
   - Exceções claras para casos inválidos

📊 VALIDAÇÕES IMPLEMENTADAS:
   ✅ Não nulo/vazio
   ✅ Formato de email válido
   ✅ Tamanho máximo (256 caracteres)
   ✅ Normalização automática

🔄 BENEFÍCIOS DO VALUE OBJECT:
   - Elimina validações repetidas
   - Garante consistência de dados
   - Facilita testes unitários
   - Melhora legibilidade do código
   - Reduz bugs relacionados a emails

📈 EVOLUÇÃO FUTURA:
   - Fácil adicionar novas validações
   - Possível integrar com serviços de validação
   - Pode incluir verificação de domínio
   - Facilita implementar whitelist/blacklist

═══════════════════════════════════════════════════════════════════════════════════
*/ 