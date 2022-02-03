# Changelog
Todas as mudanças notáveis desse projeto serão documentadas nesse arquivo.

## [0.2.0] - 02-02-2022
### Adicionado
- Projeto renomeado para Gattai Code Library
- Organização das pastas e Assets
- Adição de novos ícones para os Scripts
- SfxPlayer, um sistema para tocar SfxAudioEvents facilmente
- GameManager, um gerenciador de jogo comum e vazio de facil extensão para qualquer game
- MouseCursor, um sistema que controla o cursor do mouse e pode trocar a imagem do cursor
- Pooling, um sistema de Pool que reutiliza objetos, elimina a necessidade de destruir e instanciar os objetos
- Singletons, diversas implementações comuns do design pattern Singleton, que facilita o acesso a classes Manager
- DialogSystem, um sistema de diálogos simples
- InteractionsSystem, um sistema de interações entre o Player e objetos ou itens
- QuestSystem, um sistema de missões básico

## [0.1.1] - 18-11-2021
### Adicionado
- Sample da classe Sfx Audio Event

## [0.1.0] - 17-11-2021
### Adicionado
- AudioEvent, um ScriptableObject que pode ser herdado para configuração de assets de áudio no modo editor.
- SfxAudioEvent, uma classe que herda de Audio Event e pode ser usada para configuração avançada de efeitos sonoros, com múltiplos áudios, range de volume e ecos que são aleatórios, e é possível testar o resultado diretamente no modo editor com o clique do botão "Preview".
- RangedFloat, um tipo similar a float que pode ser configurado um range entre dois números no modo editor. 
- ButtonSfxEvent, um componente feito para testar a funcionalidade dos Sfx Audio Event facilmente em qualquer Button da UI da Unity.
- DebugConsole, um componente que possibilita digitar comandos de debug durante o modo play.
- DebugCommand Base, uma classe que pode ser herdada para criar comandos do Debug Console.
- DebugCommand, uma classe que herda de Debug Command Base e é usada para criar todos os comandos que podem utilizados no Debug Console.
- EventEmitter, um ScriptableObject que pode ser usado para disparar eventos customizados para outros GameObjects.
- EventListener, um componente que pode escutar eventos do tipo EventEmitter e responder com qualquer(s) método(s).
- ModalWindow, uma janela estilo modal que pode ser chamada a qualquer momento facilmente e ser altamente customizável.
- MusicPlayer, um componente que possibilita a criação de playlist de trilhas sonoras e pode mudar a order aleatoriamente.
- ScrollSnapRect, um componente que facilita a criação de UI mobile que pode ser arrastada para determinadas posições (exemplo: um carrossel de imagens).
- SpritesDataBase, um componente que pode ser usado como um banco de dados de sprites e elimina a necessidade de usar a funcionalidade "Resources" da Unity para isso.
- TabGroup, um componente que agrupa múltiplas abas de interfaces.
- TabButton, um botão que leva a outra aba de um TabGroup.
- TextBackgroundAutoAdjustable, um componente que ajusta o tamanho de uma imagem de background para o mesmo tamanho de um texto.