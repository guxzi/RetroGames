-- =============================================================================
-- RetroGames · Script 03 · Consultas do catálogo (Aula 1, etapa 5)
-- Estas consultas são exercícios de SQL e podem usar JOIN. No código C#, o
-- Repository NÃO usa JOIN: quem junta os dados é o Service (Espec. 3.1).
-- Troque os valores das variáveis (SET @...) para testar outros casos.
-- =============================================================================

USE retrogames;

-- -----------------------------------------------------------------------------
-- 1. Catálogo: jogos ativos com o nome da plataforma e a descrição da condição
-- -----------------------------------------------------------------------------
SELECT j.co_seq_jogo,
       j.no_jogo,
       p.no_plataforma,
       c.ds_condicao_jogo,
       j.nu_ano_lancamento,
       j.vl_preco,
       j.qt_estoque
FROM tb_jogo j
JOIN tb_plataforma    p ON p.co_seq_plataforma = j.co_plataforma
JOIN tb_condicao_jogo c ON c.co_condicao_jogo  = j.co_condicao_jogo
WHERE j.st_ativo = TRUE
ORDER BY j.no_jogo;

-- -----------------------------------------------------------------------------
-- 2. Gêneros de um jogo informado
--    (o id é buscado pelo nome só para facilitar o teste)
-- -----------------------------------------------------------------------------
SET @id_jogo = (SELECT co_seq_jogo FROM tb_jogo WHERE no_jogo = 'Super Metroid');

SELECT g.co_seq_genero, g.no_genero
FROM tb_jogo_genero jg
JOIN tb_genero g ON g.co_seq_genero = jg.co_genero
WHERE jg.co_jogo = @id_jogo
ORDER BY g.no_genero;
-- Esperado: Ação, Aventura, Plataforma

-- -----------------------------------------------------------------------------
-- 3. Jogos de um gênero informado
-- -----------------------------------------------------------------------------
SET @id_genero = (SELECT co_seq_genero FROM tb_genero WHERE no_genero = 'RPG');

SELECT j.co_seq_jogo, j.no_jogo
FROM tb_jogo_genero jg
JOIN tb_jogo j ON j.co_seq_jogo = jg.co_jogo
WHERE jg.co_genero = @id_genero
ORDER BY j.no_jogo;
-- Esperado: Chrono Trigger, Final Fantasy VII, Phantasy Star IV, Pokémon Red

-- -----------------------------------------------------------------------------
-- 4. Quantidade de jogos por plataforma, incluindo as plataformas sem jogos
--    LEFT JOIN mantém a plataforma mesmo sem jogo; COUNT(coluna) ignora NULL,
--    por isso a plataforma sem jogos aparece com 0.
-- -----------------------------------------------------------------------------
SELECT p.co_seq_plataforma,
       p.no_plataforma,
       COUNT(j.co_seq_jogo) AS qt_jogos
FROM tb_plataforma p
LEFT JOIN tb_jogo j ON j.co_plataforma = p.co_seq_plataforma
GROUP BY p.co_seq_plataforma, p.no_plataforma
ORDER BY p.no_plataforma;
-- Esperado: Sega Saturn com 0

-- -----------------------------------------------------------------------------
-- 5. Busca por parte do nome do jogo, 5 itens por página
--    OFFSET = (página - 1) * 5. O MySQL não aceita variável no LIMIT fora de
--    prepared statement, por isso as páginas estão com números fixos.
--    No C#, LIMIT e OFFSET entram como parâmetros (@tamanhoPagina, @deslocamento).
-- -----------------------------------------------------------------------------
SET @busca = 'er';   -- 7 jogos ativos contêm "er" no nome

-- Total encontrado (mesmo filtro da página)
SELECT COUNT(*) AS total
FROM tb_jogo
WHERE st_ativo = TRUE
  AND no_jogo LIKE CONCAT('%', @busca, '%');
-- Esperado: 7

-- Página 1
SELECT co_seq_jogo, no_jogo
FROM tb_jogo
WHERE st_ativo = TRUE
  AND no_jogo LIKE CONCAT('%', @busca, '%')
ORDER BY no_jogo, co_seq_jogo
LIMIT 5 OFFSET 0;
-- Esperado: 5 jogos

-- Página 2
SELECT co_seq_jogo, no_jogo
FROM tb_jogo
WHERE st_ativo = TRUE
  AND no_jogo LIKE CONCAT('%', @busca, '%')
ORDER BY no_jogo, co_seq_jogo
LIMIT 5 OFFSET 5;
-- Esperado: 2 jogos
