-- =============================================================================
-- RetroGames · Script 02 · Dados de exemplo
-- Rode logo depois do script 01, num banco recém-criado.
--
-- Conteúdo: 3 condições e 2 perfis (chaves fixas), 9 plataformas, 10 gêneros
-- e 25 jogos de consoles retrô reais, cada um com 1 a 3 gêneros.
--   * Sega Saturn e o gênero Estratégia ficam sem jogos de propósito
--     (servem para a contagem por plataforma mostrar 0).
--   * Pitfall! está inativo (st_ativo = FALSE).
--   * Chrono Trigger está com estoque zero.
--   * Nenhum usuário é inserido: o administrador é criado pela API.
--
-- Os jogos e os vínculos usam o NOME da plataforma/gênero para achar o id,
-- assim o script não depende da ordem do AUTO_INCREMENT.
-- =============================================================================

USE retrogames;

-- Condições de jogo (chave fixa, mesmos valores de CondicaoJogoIds)
INSERT INTO tb_condicao_jogo (co_condicao_jogo, ds_condicao_jogo) VALUES
    (1, 'Novo'),
    (2, 'Usado'),
    (3, 'Recondicionado');

-- Perfis (chave fixa, mesmos valores de PerfilIds)
INSERT INTO tb_perfil (co_perfil, ds_perfil) VALUES
    (1, 'Administrador'),
    (2, 'Cliente');

-- Plataformas
INSERT INTO tb_plataforma (no_plataforma, no_fabricante, nu_ano_lancamento) VALUES
    ('Atari 2600',     'Atari',    1977),
    ('NES',            'Nintendo', 1985),
    ('Master System',  'Sega',     1986),
    ('Mega Drive',     'Sega',     1988),
    ('Game Boy',       'Nintendo', 1989),
    ('Super Nintendo', 'Nintendo', 1990),
    ('PlayStation',    'Sony',     1994),
    ('Sega Saturn',    'Sega',     1994),
    ('Nintendo 64',    'Nintendo', 1996);

-- Gêneros
INSERT INTO tb_genero (no_genero) VALUES
    ('Ação'),
    ('Aventura'),
    ('Corrida'),
    ('Esporte'),
    ('Estratégia'),
    ('Luta'),
    ('Plataforma'),
    ('Puzzle'),
    ('RPG'),
    ('Tiro');

-- Jogos (condição: 1 Novo, 2 Usado, 3 Recondicionado)
INSERT INTO tb_jogo (no_jogo, ds_jogo, nu_ano_lancamento, vl_preco, qt_estoque,
                     st_ativo, dt_cadastro, co_plataforma, co_condicao_jogo)
SELECT d.nome, d.descricao, d.ano, d.preco, d.estoque,
       d.ativo, UTC_TIMESTAMP(), p.co_seq_plataforma, d.condicao
FROM (
              SELECT 'Pitfall!' AS nome, 'Aventura na selva com Pitfall Harry.' AS descricao, 1982 AS ano, 89.90 AS preco, 2 AS estoque, FALSE AS ativo, 'Atari 2600' AS plataforma, 2 AS condicao
    UNION ALL SELECT 'Super Mario Bros.', 'O clássico que definiu os jogos de plataforma.', 1985, 149.90, 4, TRUE, 'NES', 2
    UNION ALL SELECT 'The Legend of Zelda', 'A primeira aventura de Link em Hyrule.', 1986, 179.90, 2, TRUE, 'NES', 3
    UNION ALL SELECT 'Alex Kidd in Miracle World', 'O mascote da Sega antes do Sonic.', 1986, 99.90, 3, TRUE, 'Master System', 2
    UNION ALL SELECT 'Sonic the Hedgehog', 'Velocidade e anéis em Green Hill Zone.', 1991, 129.90, 5, TRUE, 'Mega Drive', 2
    UNION ALL SELECT 'Streets of Rage 2', 'Beat ''em up com trilha de Yuzo Koshiro.', 1992, 159.90, 2, TRUE, 'Mega Drive', 2
    UNION ALL SELECT 'Phantasy Star IV', NULL, 1993, 349.90, 1, TRUE, 'Mega Drive', 3
    UNION ALL SELECT 'Tetris', 'Peças que caem e linhas que somem.', 1989, 79.90, 6, TRUE, 'Game Boy', 2
    UNION ALL SELECT 'Pokémon Red', 'Capture os 151 Pokémon da primeira geração.', 1996, 249.90, 2, TRUE, 'Game Boy', 2
    UNION ALL SELECT 'Super Mario World', 'Mario e Yoshi em Dinosaur Land.', 1990, 199.90, 3, TRUE, 'Super Nintendo', 2
    UNION ALL SELECT 'The Legend of Zelda: A Link to the Past', 'Link entre o Light World e o Dark World.', 1991, 229.90, 2, TRUE, 'Super Nintendo', 3
    UNION ALL SELECT 'Street Fighter II', 'O jogo de luta que lotou os fliperamas.', 1992, 149.90, 4, TRUE, 'Super Nintendo', 2
    UNION ALL SELECT 'Top Gear', 'Corrida com uma das trilhas mais lembradas do SNES.', 1992, 119.90, 5, TRUE, 'Super Nintendo', 2
    UNION ALL SELECT 'Super Metroid', 'Samus explora o planeta Zebes.', 1994, 279.90, 1, TRUE, 'Super Nintendo', 3
    UNION ALL SELECT 'Donkey Kong Country', 'Gráficos pré-renderizados que impressionaram em 1994.', 1994, 169.90, 3, TRUE, 'Super Nintendo', 2
    UNION ALL SELECT 'International Superstar Soccer', NULL, 1994, 99.90, 4, TRUE, 'Super Nintendo', 2
    UNION ALL SELECT 'Chrono Trigger', 'RPG com viagem no tempo e múltiplos finais.', 1995, 599.90, 0, TRUE, 'Super Nintendo', 3
    UNION ALL SELECT 'Crash Bandicoot', 'O marsupial laranja do PlayStation.', 1996, 109.90, 5, TRUE, 'PlayStation', 2
    UNION ALL SELECT 'Resident Evil', 'Sobrevivência na mansão Spencer.', 1996, 189.90, 2, TRUE, 'PlayStation', 2
    UNION ALL SELECT 'Final Fantasy VII', 'Cloud e a AVALANCHE contra a Shinra.', 1997, 219.90, 3, TRUE, 'PlayStation', 2
    UNION ALL SELECT 'Gran Turismo', 'Simulador de corrida com centenas de carros.', 1997, 89.90, 6, TRUE, 'PlayStation', 2
    UNION ALL SELECT 'Super Mario 64', 'Mario em 3D no castelo da Peach.', 1996, 259.90, 2, TRUE, 'Nintendo 64', 2
    UNION ALL SELECT 'Mario Kart 64', 'Corrida para até quatro jogadores.', 1996, 199.90, 3, TRUE, 'Nintendo 64', 2
    UNION ALL SELECT 'GoldenEye 007', 'Tiro em primeira pessoa com o agente 007.', 1997, 179.90, 2, TRUE, 'Nintendo 64', 3
    UNION ALL SELECT 'The Legend of Zelda: Ocarina of Time', 'Link viaja entre passado e futuro com a ocarina.', 1998, 299.90, 1, TRUE, 'Nintendo 64', 1
) AS d
JOIN tb_plataforma p ON p.no_plataforma = d.plataforma;

-- Vínculos jogo x gênero (1 a 3 gêneros por jogo)
INSERT INTO tb_jogo_genero (co_jogo, co_genero)
SELECT j.co_seq_jogo, g.co_seq_genero
FROM (
              SELECT 'Pitfall!' AS jogo, 'Plataforma' AS genero
    UNION ALL SELECT 'Pitfall!', 'Aventura'
    UNION ALL SELECT 'Super Mario Bros.', 'Plataforma'
    UNION ALL SELECT 'The Legend of Zelda', 'Ação'
    UNION ALL SELECT 'The Legend of Zelda', 'Aventura'
    UNION ALL SELECT 'Alex Kidd in Miracle World', 'Plataforma'
    UNION ALL SELECT 'Sonic the Hedgehog', 'Plataforma'
    UNION ALL SELECT 'Sonic the Hedgehog', 'Ação'
    UNION ALL SELECT 'Streets of Rage 2', 'Ação'
    UNION ALL SELECT 'Streets of Rage 2', 'Luta'
    UNION ALL SELECT 'Phantasy Star IV', 'RPG'
    UNION ALL SELECT 'Tetris', 'Puzzle'
    UNION ALL SELECT 'Pokémon Red', 'RPG'
    UNION ALL SELECT 'Pokémon Red', 'Aventura'
    UNION ALL SELECT 'Super Mario World', 'Plataforma'
    UNION ALL SELECT 'The Legend of Zelda: A Link to the Past', 'Ação'
    UNION ALL SELECT 'The Legend of Zelda: A Link to the Past', 'Aventura'
    UNION ALL SELECT 'Street Fighter II', 'Luta'
    UNION ALL SELECT 'Top Gear', 'Corrida'
    UNION ALL SELECT 'Super Metroid', 'Ação'
    UNION ALL SELECT 'Super Metroid', 'Aventura'
    UNION ALL SELECT 'Super Metroid', 'Plataforma'
    UNION ALL SELECT 'Donkey Kong Country', 'Plataforma'
    UNION ALL SELECT 'International Superstar Soccer', 'Esporte'
    UNION ALL SELECT 'Chrono Trigger', 'RPG'
    UNION ALL SELECT 'Crash Bandicoot', 'Plataforma'
    UNION ALL SELECT 'Resident Evil', 'Ação'
    UNION ALL SELECT 'Resident Evil', 'Aventura'
    UNION ALL SELECT 'Final Fantasy VII', 'RPG'
    UNION ALL SELECT 'Gran Turismo', 'Corrida'
    UNION ALL SELECT 'Gran Turismo', 'Esporte'
    UNION ALL SELECT 'Super Mario 64', 'Plataforma'
    UNION ALL SELECT 'Super Mario 64', 'Aventura'
    UNION ALL SELECT 'Mario Kart 64', 'Corrida'
    UNION ALL SELECT 'GoldenEye 007', 'Tiro'
    UNION ALL SELECT 'GoldenEye 007', 'Ação'
    UNION ALL SELECT 'The Legend of Zelda: Ocarina of Time', 'Ação'
    UNION ALL SELECT 'The Legend of Zelda: Ocarina of Time', 'Aventura'
) AS v
JOIN tb_jogo j ON j.no_jogo = v.jogo
JOIN tb_genero g ON g.no_genero = v.genero;

-- -----------------------------------------------------------------------------
-- Conferência (itens "Confira" da etapa 4)
-- -----------------------------------------------------------------------------
SELECT
    (SELECT COUNT(*) FROM tb_plataforma) AS plataformas,   -- esperado: 9  (mínimo 6)
    (SELECT COUNT(*) FROM tb_genero)     AS generos,       -- esperado: 10 (mínimo 8)
    (SELECT COUNT(*) FROM tb_jogo)       AS jogos,         -- esperado: 25 (mínimo 20)
    (SELECT COUNT(*) FROM tb_usuario)    AS usuarios;      -- esperado: 0

-- Jogos sem gênero (esperado: nenhuma linha)
SELECT j.co_seq_jogo, j.no_jogo
FROM tb_jogo j
LEFT JOIN tb_jogo_genero jg ON jg.co_jogo = j.co_seq_jogo
WHERE jg.co_jogo IS NULL;

-- Jogo inativo e jogo com estoque zero (esperado: Pitfall! e Chrono Trigger)
SELECT no_jogo, st_ativo, qt_estoque
FROM tb_jogo
WHERE st_ativo = FALSE OR qt_estoque = 0;
