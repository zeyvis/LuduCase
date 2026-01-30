## Prompt 1: Core Interaction Contracts (IInteractable + core types)

**Araç:** ChatGPT  
**Tarih:** 2026-01-30 18.30

**Prompt:**
> Unity intern case için modüler bir interaction system kuruyorum.  
> IInteractable interface tasarımını ve yanında ihtiyaç duyacağım core yapıları interaction type, UI prompt data, context clean olacak şekilde öner
> Ayrıca bu yapıların neden gerekli olduğunu mesela detector tarafında nasıl kullanılacağını kısa ama net açıkla

**Alınan Cevap (Özet):**
> - Tüm etkileşilebilir objeler için tek bir kontrat olarak IInteractable önerildi
> - InteractionType ile “Instant/Hold/Toggle” davranışları ayrıştırıldı
> - InteractionPromptData ile UI tarafına prompt/hold bar/cannot interact reason gibi bilgiler tek yerden taşınabilir hale getirildi.  
> - InteractionContext ile “kim etkileşiyor” bilgisi standartlaştırıldı (ileride inventory vb. eklenebilir)

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Case dokümanında -IInteractable interface- açıkça beklendiği için interface ismini korudum.  
> Core yapıların kapsamını minimal tuttum 


---

## Prompt 2: Detection yöntemi seçimi (Raycast vs Trigger) + trade-off

**Araç:** ChatGPT  
**Tarih:** 2026-01-30 18.45

**Prompt:**
> InteractionDetector tarafında raycast mı trigger-based detection mı kullanmalıyım?
> Dokümanda ikisine de izin verildiğini gördüm. Ben raycast seçmek istiyorum
> Sebep olarak  tek target, daha az state veya list yönetimi  gibi düşündüm
> Bu düşünce doğru mu? Kısaca nedenleriyle açıkla ve riskleri söyle.

**Alınan Cevap (Özet):**
> - Trigger yaklaşımı genelde birden çok interactable’ı aynı anda yakalayabileceği için liste yönetimi ve en yakın seçimi gibi ek mantık gerektirir  
> - Performans olarak iki yaklaşım da küçük demoda sorun yaratmazfakat raycast ile kontrol akışı daha sade ve hataya daha kapalı olur.

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Raycast seçimini uygulayacağım çünkü casein tek nesne ile etkileşim ve en yakın seçim beklentisini daha az kodla ve güvenli şekilde karşılıyor.  
> Trigger seçseydim aynı anda birden fazla objeyi yönetmek için ekstra liste mantığı yazmam gerekebilir.


---

## Prompt 3: XML documentation / summary iyileştirme (daha resmi, sektör dili)

**Araç:** ChatGPT  
**Tarih:** 2026-01-30  19.01

**Prompt:**
> Core scriptlerdeki XML documrtation (/// <summary> …) açıklamalarını sektör diliyle daha resmi ve net hale getirmek istiyorum.  
> Gereksiz uzunluk olmasın ama public API ler açıklayıcı olsun.  


**Alınan Cevap (Özet):**
> - Summarylerin kısa, net ve fiil odaklı olması önerildi (örn. “Represents…”, “Provides…”, “Defines…”).  
> - Summarylerimi uygun olup olmadığını kontrol etti

**Nasıl Kullandım:**
- [ ] Direkt kullandım
- [x] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> IInteractable ve core tiplerdeki summary metinlerini daha resmi hale getirip tekrar eden cümleleri kısalttım.  
> Bunu yapma sebebim diğer çalışanlara zorluk çıkarmamak içind.


---

## Prompt 4: Prompt başlıklarını iyileştirme (okunabilirlik ve süreç takibi için)

**Araç:** ChatGPT  
**Tarih:** 2026-01-30  19.10

**Prompt:**
> PROMPTS.md içindeki başlıkların lead developer tarafından hızlı tarandığında süreci net anlatmasını istiyorum  
> Prompt başlıklarını daha açıklayıcı ve profesyonel hale getirmek için öneriler üret
> Her başlığın hangi problem çözüldüğünü ve hangi sistem parçasını etkilediğini yansıtmasına dikkat et.

**Alınan Cevap (Özet):**
> - Prompt başlıklarının kısa ama bağlam içermesi önerildi.  

**Nasıl Kullandım:**
- [x] Direkt kullandım
- [ ] Adapte ettim
- [ ] Reddettim

**Açıklama:**
> Prompt başlıklarını daha açıklayıcı hale getirerek case yi inceleyen kişinin hangi adımda hangi kararı verdiğimi daha hızlı görmesini istedim ondan dolayı kullandım.

